using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Shared;

namespace Infrastructure.Services.Shared
{
    public class RouteService : IRoute
    {
        private readonly IAsyncRepository<Route> _context;

        public RouteService(IAsyncRepository<Route> context)
        {
            _context = context;
        }

        public async Task<Route> GetByCitiesIdAsync(Guid startCityId, Guid finishCityId) =>
            await _context.FirstOrDefaultAsync(r =>
                r.StartCityId == startCityId &&
                r.FinishCityId == finishCityId);

        public async Task<Route> GetByCitiesNameAsync(string startCityName, string finishCityName) =>
            await _context.FirstOrDefaultAsync(r =>
                r.StartCity.Name == startCityName &&
                r.FinishCity.Name == finishCityName);
    }
}