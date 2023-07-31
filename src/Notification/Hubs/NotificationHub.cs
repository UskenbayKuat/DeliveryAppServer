using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Models.Dtos.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Notification.Interfaces;

namespace Notification.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IChatHub _chatHub;
        private readonly ILogger<NotificationHub> _logger;
        private readonly INotify _notify;
        private readonly IDeliveryCommand _deliveryCommand;

        public NotificationHub(
            IChatHub chatHub,
            ILogger<NotificationHub> logger,
            INotify notify,
            IDeliveryCommand deliveryCommand)
        {
            _chatHub = chatHub;
            _logger = logger;
            _notify = notify;
            _deliveryCommand = deliveryCommand;
        }

        public async Task ReceiveDriverLocation(LocationDto request)
        {
            var locationInfo = await _deliveryCommand
                .UpdateLocationAsync(request.SetUserId(Context.GetHttpContext().Items["UserId"]?.ToString()));
            await _notify.SendDriverLocationToClientsAsync(request.UserId, locationInfo);
            _logger.LogInformation($"{request.DriverName} : {DateTime.Now:G}");
        }

        public override async Task<Task> OnConnectedAsync()
        {
            await _chatHub.ConnectedAsync(Context.GetHttpContext().Items["UserId"]?.ToString(), Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            await _chatHub.DisconnectedAsync(Context.GetHttpContext().Items["UserId"]?.ToString(), Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}