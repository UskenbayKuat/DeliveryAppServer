using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ChatHubServices
{
    public class ChatHubService : IChatHub
    {
        private readonly IContext _context;
        private readonly IOrderQuery _orderQuery;

        public ChatHubService(IContext context, IOrderQuery orderQuery)
        {
            _context = context;
            _orderQuery = orderQuery;
        }

        public async Task ConnectedAsync(string userId, string connectId)
        {
            var chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == userId)
                          ?? await CreateChatHubAsync(userId, connectId);
            await _context.UpdateAsync(chatHub.UpdateConnectId(connectId));
        }

        public async Task<string> GetConnectionIdAsync(string userId, CancellationToken cancellationToken)
        {
            var chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == userId);
            return chatHub?.ConnectionId;
        }

        public async Task<List<string>> GetConnectionIdListAsync(string driverUserId)
        {
            var connectionIds = new List<string>();
            var orders = await _orderQuery.GetByDriverUserIdAsync(driverUserId);
            foreach (var order in orders)
            {
                var chatHub = await  _context.FindAsync<ChatHub>(c => c.UserId == order.Client.UserId);
                connectionIds.Add(chatHub?.ConnectionId);
            }
            return connectionIds;
        }

        public async Task DisconnectedAsync(string userId, string connectId)
        {
            ChatHub chatHub;
            if (!string.IsNullOrEmpty(userId))
            {
                chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == userId);
            }
            else if (!string.IsNullOrEmpty(userId))
            {
                chatHub = await _context.FindAsync<ChatHub>(c => c.ConnectionId == connectId);
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
                throw new NullReferenceException();
            }
            var chatHub = new ChatHub(userId, connectId);
            await _context.AddAsync(chatHub);
            return chatHub;
        }
    }
}