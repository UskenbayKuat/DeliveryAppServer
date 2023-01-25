using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.Config.Attributes;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PublicApi.Commands;

namespace PublicApi.HubNotification
{
    [Authorize]
    public class Notification : Hub
    {
        private readonly IChatHub _chatHub;
        private readonly IMediator _mediator;
        
        public Notification(IChatHub chatHub, IMediator mediator)
        {
            _chatHub = chatHub;
            _mediator = mediator;
        }

        public async Task ReceiveDriverLocation(LocationCommand request)
        {
            await _mediator.Send(request.SetUserId(Context.GetHttpContext().Items["UserId"]?.ToString()));
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