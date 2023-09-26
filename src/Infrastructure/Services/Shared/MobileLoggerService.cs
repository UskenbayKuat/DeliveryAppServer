using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Services.Shared
{
    public class MobileLoggerService : IMobileLogger
    {
        private readonly IAsyncRepository<MobileLogger> _context;

        public MobileLoggerService(IAsyncRepository<MobileLogger> context)
        {
            _context = context;
        }

        public Task AddAsync(MobileLoggerDto dto)
        {
            _context
        }
    }
}
