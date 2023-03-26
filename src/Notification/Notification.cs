using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Interfaces.LocationInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Interfaces;

namespace Notification
{
    public class Notification : Hub
    {
        private readonly IChatHub _chatHub;
        private readonly ILocation _location;
        private readonly ILogger<Notification> _logger;
        private readonly INotify _notify;

        public Notification(IChatHub chatHub, ILogger<Notification> logger, ILocation location, INotify notify)
        {
            _chatHub = chatHub;
            _logger = logger;
            _location = location;
            _notify = notify;
        }

        public async Task ReceiveDriverLocation(LocationInfo request)
        {
            var locationInfo = await _location.UpdateDriverLocationAsync(request.SetUserId(Context.GetHttpContext().Items["UserId"]?.ToString()));
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
            await _chatHub.DisconnectedAsync(Context.GetHttpContext().Items["UserId"]?.ToString(),Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}