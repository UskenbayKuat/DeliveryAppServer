using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.RouteInterfaces;

namespace Infrastructure.Services.RouteServices
{
    public class RouteService : IRoute
    {
        private readonly IAsyncRepository<Route> _context;

        public RouteService(IAsyncRepository<Route> context)
        {
            _context = context;
        }

        public async Task<Route> GetByCitiesIdAsync(int startCityId, int finishCityId) =>
            await _context.FirstOrDefaultAsync(r =>
                r.StartCityId == startCityId &&
                r.FinishCityId == finishCityId);
    }
}