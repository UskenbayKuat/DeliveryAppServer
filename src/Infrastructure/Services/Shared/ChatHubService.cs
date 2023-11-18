using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Interfaces.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services.Shared
{
    public class ChatHubService : IChatHub
    {
        private readonly IOrderQuery _orderQuery;
        private readonly IAsyncRepository<ChatHub> _context;
        public ChatHubService(IAsyncRepository<ChatHub> context, IOrderQuery orderQuery)
        {
            _context = context;
            _orderQuery = orderQuery;
        }

        public async Task ConnectedAsync(Guid? userId, string connectId)
        {
            var chatHub = await _context.FirstOrDefaultAsync(c => c.UserId == userId)
                          ?? await CreateChatHubAsync(userId, connectId);
            await _context.UpdateAsync(chatHub.UpdateConnectId(connectId));
        }

        public async Task<string> GetConnectionIdAsync(Guid? userId, CancellationToken cancellationToken)
        {
            var chatHub = await _context.FirstOrDefaultAsync(c => userId.HasValue && c.UserId == userId, cancellationToken);
            return chatHub?.ConnectionId;
        }

        public async Task<List<string>> GetConnectionIdListAsync(Guid driverUserId)
        {
            var connectionIds = new List<string>();
            var orders = await _orderQuery.GetByDriverUserIdAsync(driverUserId);
            foreach (var order in orders)
            {
                var chatHub = await _context.FirstOrDefaultAsync(c => c.UserId == order.Client.User.Id);
                if (chatHub != null && !string.IsNullOrEmpty(chatHub.ConnectionId))
                {
                    connectionIds.Add(chatHub.ConnectionId);
                }
            }
            return connectionIds;
        }

        public async Task DisconnectedAsync(Guid? userId, string connectId)
        {
            ChatHub chatHub;
            if (userId.HasValue)
            {
                chatHub = await _context.FirstOrDefaultAsync(c => c.UserId == userId.Value);
            }
            else if (!string.IsNullOrEmpty(connectId))
            {
                chatHub = await _context.FirstOrDefaultAsync(c => c.ConnectionId == connectId);
            }
            else
            {
                throw new HubException();
            }
            await _context.UpdateAsync(chatHub?.RemoveConnectId());
        }

        private async Task<ChatHub> CreateChatHubAsync(Guid? userId, string connectId)
        {
            if (!userId.HasValue)
            {
                throw new HubException($"Error connect Hub userId -> null, connectId: {connectId}");
            }
            var chatHub = new ChatHub(userId.Value, connectId);
            await _context.AddAsync(chatHub);
            return chatHub;
        }
    }
}