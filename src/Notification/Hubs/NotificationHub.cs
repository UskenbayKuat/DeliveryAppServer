using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Drivers;
using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Notification.Services;

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
            _logger.LogInformation($"{DateTime.Now:g}:  Connect: ConnectingId: {Context.ConnectionId}, UserId: {Context.GetHttpContext().Items["UserId"]?.ToString()}");
            _ = Guid.TryParse(Context.GetHttpContext().Items["UserId"]?.ToString(), out var userId);
            await _chatHub.ConnectedAsync(userId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"{DateTime.Now:g}: Disconnect: ConnectingId: {Context.ConnectionId}, UserId: {Context.GetHttpContext().Items["UserId"]?.ToString()}");
            _ = Guid.TryParse(Context.GetHttpContext().Items["UserId"]?.ToString(), out var userId);
            await _chatHub.DisconnectedAsync(userId, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string message)
        {
            await Clients.Others.SendAsync("SendToDriver", message);
        }
    }
}