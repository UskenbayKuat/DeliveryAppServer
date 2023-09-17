using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Drivers;
using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
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
            await _chatHub.ConnectedAsync(Context.GetHttpContext().Items["UserId"]?.ToString(), Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"{DateTime.Now:g}: Disconnect: ConnectingId: {Context.ConnectionId}, UserId: {Context.GetHttpContext().Items["UserId"]?.ToString()}");
            await _chatHub.DisconnectedAsync(Context.GetHttpContext().Items["UserId"]?.ToString(), Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("g") + ": " + message);
            await Clients.Others.SendAsync("SendToDriver", message);
        }
    }
}