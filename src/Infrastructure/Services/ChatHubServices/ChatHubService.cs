using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ChatHubServices
{
    public class ChatHubService : IChatHub
    {
        private readonly AppDbContext _db;

        public ChatHubService(AppDbContext db)
        {
            _db = db;
        }

        public async Task ConnectedAsync(string userId, string connectId)
        {
            var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == userId)
                          ?? await CreateChatHubAsync(userId, connectId);
            _db.ChatHubs.Update(chatHub.UpdateConnectId(connectId));
            await _db.SaveChangesAsync();
        }

        private async Task<ChatHub> CreateChatHubAsync(string userId, string connectId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NullReferenceException();
            }
            var chatHub = new ChatHub(userId, connectId);
            await _db.ChatHubs.AddAsync(chatHub);
            return chatHub;
        }

        public async Task DisconnectedAsync(string connectId)
        {
            var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.ConnectionId == connectId);
            _db.ChatHubs.Update(chatHub.RemoveConnectId());
            await _db.SaveChangesAsync();
        }
    }
}