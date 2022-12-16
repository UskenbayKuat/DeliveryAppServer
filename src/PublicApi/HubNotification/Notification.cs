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
        private readonly IChatHub _chatHub;
        
        public Notification(IChatHub chatHub)
        {
            _chatHub = chatHub;
        }
        public override async Task<Task> OnConnectedAsync()
        {
            await _chatHub.ConnectedAsync(Context.GetHttpContext().Items["UserId"]?.ToString(), Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            await _chatHub.DisconnectedAsync(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}