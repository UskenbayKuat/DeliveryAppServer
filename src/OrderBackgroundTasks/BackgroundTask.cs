using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Interfaces.Drivers;
using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notification.Services;

namespace OrderBackgroundTasks
{
    public class BackgroundTask : BackgroundService
    {    
        private readonly ILogger<BackgroundTask> _logger;
        private readonly IBackgroundTaskQueue _backgroundQueue;
        private readonly IServiceScopeFactory _serviceProvider;
        public BackgroundTask(
            IBackgroundTaskQueue backgroundQueue, 
            IServiceScopeFactory serviceProvider, 
            ILogger<BackgroundTask> logger)
        {
            _backgroundQueue = backgroundQueue;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BackgroundTask service running.");
            while (!stoppingToken.IsCancellationRequested)
            {
                var backgroundOrder = await _backgroundQueue.DequeueAsync(stoppingToken);
                if (backgroundOrder is null) continue;
                while (backgroundOrder.WaitingPeriodTime > DateTime.Now)
                {
                    _logger.LogInformation("Waiting order state. {0} : {1}", backgroundOrder.WaitingPeriodTime.ToString("T"), DateTime.Now.ToString("T"));
                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
                await OrderHandlerAsync(backgroundOrder, stoppingToken);
            }
        }

        private async Task OrderHandlerAsync(BackgroundOrder backgroundOrder, CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var orderCommand = serviceProvider.GetService<IOrderCommand>();
            if (!await orderCommand.IsOnReview(backgroundOrder))
            {
                return;
            }
            var deliveryCommand = serviceProvider.GetService<IDeliveryCommand>();
            var order = await orderCommand.RejectAsync(backgroundOrder.OrderId);
            _logger.LogInformation($"DeliverId: {backgroundOrder.DeliveryId} reject order: {backgroundOrder.OrderId}");
            var delivery = await deliveryCommand.FindIsActiveDeliveryAsync(order);
            if (delivery is null)
            {
                return;
            }
            await orderCommand.SetDeliveryAsync(order, delivery);
            _logger.LogInformation($"Order: {backgroundOrder.OrderId} state OnReview from DeliverId: {backgroundOrder.DeliveryId}");
            var notify = serviceProvider.GetService<INotify>();
            await notify.SendToDriverAsync(delivery.Driver.User.Id, stoppingToken);
        }
    }
}