using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Clients;

namespace Infrastructure.Services.Clients
{
    public class ClientService : IClient
    {
        private readonly IAsyncRepository<Client> _context;

        public ClientService(IAsyncRepository<Client> context)
        {
            _context = context;
        }

        public async Task<Client> GetByUserId(string userId)
        {
            return await _context.FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}