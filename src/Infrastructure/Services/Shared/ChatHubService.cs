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

        public async Task ConnectedAsync(string userId, string connectId)
        {
            var chatHub = await _context.FirstOrDefaultAsync(c => c.UserId == userId)
                          ?? await CreateChatHubAsync(userId, connectId);
            await _context.UpdateAsync(chatHub.UpdateConnectId(connectId));
        }

        public async Task<string> GetConnectionIdAsync(string userId, CancellationToken cancellationToken)
        {
            var chatHub = await _context.FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
            return chatHub?.ConnectionId;
        }

        public async Task<List<string>> GetConnectionIdListAsync(string driverUserId)
        {
            var connectionIds = new List<string>();
            var orders = await _orderQuery.GetByDriverUserIdAsync(driverUserId);
            foreach (var order in orders)
            {
                var chatHub = await _context.FirstOrDefaultAsync(c => c.UserId == order.Client.UserId);
                if (chatHub != null && string.IsNullOrEmpty(chatHub.ConnectionId))
                {
                    connectionIds.Add(chatHub.ConnectionId);
                }
            }
            return connectionIds;
        }

        public async Task DisconnectedAsync(string userId, string connectId)
        {
            ChatHub chatHub;
            if (!string.IsNullOrEmpty(userId))
            {
                chatHub = await _context.FirstOrDefaultAsync(c => c.UserId == userId);
            }
            else if (!string.IsNullOrEmpty(userId))
            {
                chatHub = await _context.FirstOrDefaultAsync(c => c.ConnectionId == connectId);
            }
            else
            {
                throw new HubException();
            }
            await _context.UpdateAsync(chatHub?.RemoveConnectId());
        }

        private async Task<ChatHub> CreateChatHubAsync(string userId, string connectId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException($"Error connect Hub userId -> null, connectId: {connectId}");
            }
            var chatHub = new ChatHub(userId, connectId);
            await _context.AddAsync(chatHub);
            return chatHub;
        }
    }
}