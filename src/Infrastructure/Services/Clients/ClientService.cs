using System;
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

        public async Task<Client> GetByUserId(Guid userId)
        {
            return await _context.FirstOrDefaultAsync(c => c.User.Id == userId);
        }
    }
}