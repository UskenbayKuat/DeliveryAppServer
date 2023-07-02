using System.Threading.Tasks;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.StateInterfaces;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;

namespace Infrastructure.Services.StateServices
{
    public class StateService : IState
    {
        private readonly IAsyncRepository<State> _context;

        public StateService(IAsyncRepository<State> context)
        {
            _context = context;
        }

        public async Task<State> GetByStateAsync(GeneralState state) =>
            await _context.FirstOrDefaultAsync(s => s.StateValue == state);
    }
}