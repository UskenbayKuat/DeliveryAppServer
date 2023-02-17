using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundTasks
{
    public class BackgroundTask : BackgroundService
    {    
        private readonly ILogger<BackgroundTask> _logger;
        private readonly IBackgroundTaskQueue _backgroundQueue;
        private readonly IServiceScopeFactory _serviceProvider;
        public BackgroundTask(IBackgroundTaskQueue backgroundQueue, IServiceScopeFactory serviceProvider, ILogger<BackgroundTask> logger)
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

        private async ValueTask OrderHandlerAsync(BackgroundOrder backgroundOrder, CancellationToken stoppingToken)
        {
            var serviceProvider = _serviceProvider.CreateScope().ServiceProvider;
            var dbContext = serviceProvider.GetService<AppDbContext>();
            var order = await OrderAsync(dbContext, backgroundOrder);
            if (CheckOrderState(order, backgroundOrder))
            {
                var orderHandler = serviceProvider.GetService<IOrderHandler>();
                await orderHandler.RejectedHandlerAsync(order.Id, stoppingToken);
                _logger.LogInformation("Complete work!");
            }
        }
        
        private async Task<Order> OrderAsync(AppDbContext dbContext,BackgroundOrder backgroundOrder) =>
            await dbContext.Orders
                .Include(o => o.State)
                .Include(o => o.Delivery.RouteTrip)
                .FirstOrDefaultAsync(o =>
                    o.Id == backgroundOrder.OrderId && 
                    o.Delivery.Id == backgroundOrder.DeliveryId);  //TODO builder

        private bool CheckOrderState(Order order, BackgroundOrder delivery) =>
            order?.State.Id == (int)GeneralState.OnReview && 
            order.Delivery.Id == delivery.DeliveryId;
    }
}