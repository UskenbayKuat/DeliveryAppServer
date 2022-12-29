using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ChatHubServices
{
    public class ChatHubService : IChatHub
    {
        private readonly IContext _context;

        public ChatHubService(IContext context)
        {
            _context = context;
        }

        public async Task ConnectedAsync(string userId, string connectId)
        {
            var chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == userId)
                          ?? await CreateChatHubAsync(userId, connectId);
            await _context.UpdateAsync(chatHub.UpdateConnectId(connectId));
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

        public async Task DisconnectedAsync(string connectId)
        {
            var chatHub = await _context.FindAsync<ChatHub>(c => c.ConnectionId == connectId);
            await _context.UpdateAsync(chatHub.RemoveConnectId());
        }
    }
}