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
        private readonly IOrderContextBuilder _orderContextBuilder;

        public Notification(IChatHub chatHub, ILogger<Notification> logger, ILocation location, INotify notify, IOrderContextBuilder orderContextBuilder)
        {
            _chatHub = chatHub;
            _logger = logger;
            _location = location;
            _notify = notify;
            _orderContextBuilder = orderContextBuilder;
        }

        public async Task ReceiveDriverLocation(LocationInfo request)
        {
            if (string.IsNullOrEmpty(Context.GetHttpContext().Items["UserId"].ToString()))
            {
                throw new HubException("Data not correct,request has not userId");
            }
            var orders = await _orderContextBuilder.ClientAndDeliveryBuilder()
                .Build()
                .Where(o => o.Delivery.Driver.UserId == Context.GetHttpContext().Items["UserId"].ToString())
                .ToListAsync();
            var locationInfo = await _location.SendDriverLocationAsync(request.SetUserId(Context.GetHttpContext().Items["UserId"]?.ToString()), orders.FirstOrDefault()!.Delivery.Id);
            await _notify.SendDriverLocationToClientsAsync(orders, locationInfo);
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