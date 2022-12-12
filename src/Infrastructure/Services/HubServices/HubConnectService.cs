using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.HubServices
{
    public class HubConnectService : IHubConnect
    {
        private readonly AppDbContext _db;

        public HubConnectService(AppDbContext db)
        {
            _db = db;
        }

        public async Task ConnectedUser(string userId, string connectId)
        {
            var chatHub = !string.IsNullOrEmpty(userId)
                ? await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == userId)
                : throw new HubException();
            if (chatHub is not null)
                _db.ChatHubs.Update(chatHub.UpdateConnectId(connectId));
            else
                await _db.ChatHubs.AddAsync(new ChatHub(userId, connectId));
            await _db.SaveChangesAsync();
        }

        public async Task DisconnectedUser(string connectId)
        {
            var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.ConnectionId == connectId) 
                          ?? throw new HubException();
            _db.ChatHubs.Update(chatHub.RemoveConnectId());
            await _db.SaveChangesAsync();
        }
    }
}