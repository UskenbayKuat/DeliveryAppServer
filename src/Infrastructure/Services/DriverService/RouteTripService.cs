using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.DriverService
{
    public class RouteTripService : IRouteTrip
    {
        private readonly AppDbContext _db;

        public RouteTripService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<ActionResult> CreateRouteTrip(RouteInfo info, CancellationToken cancellationToken)
        {
            var startRoute = _db.Cities.FirstOrDefault(r => r.Id == info.StartCityId);
            var finishRoute = _db.Cities.FirstOrDefault(r => r.Id == info.FinishCityId);
            if (startRoute is null || finishRoute is null)
            {
                return await Task.FromResult<ActionResult>(new BadRequestResult());
            }
            var trip = new RouteTrip()
            {
                StartCity = startRoute,
                StartCityId = startRoute.Id,
                FinishCity = finishRoute,
                FinishCityId = finishRoute.Id,
                TripTime = info.TripTime,
                DriverId = info.DriverId,
            };
            await _db.RouteTrips.AddAsync(trip,cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return await Task.FromResult<ActionResult>(new OkObjectResult(trip));
        }
    }
}