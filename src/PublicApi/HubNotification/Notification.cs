using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.SignalR;

namespace PublicApi.HubNotification
{
    [Authorize]
    public class Notification : Hub
    {
        private readonly IHubConnect _hubConnect;
        
        public Notification(IHubConnect hubConnect)
        {
            _hubConnect = hubConnect;
        }
        public override async Task<Task> OnConnectedAsync()
        {
            await _hubConnect.ConnectedUser(Context.GetHttpContext().Items["UserId"]?.ToString(), Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            await _hubConnect.DisconnectedUser(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}