using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverService
{
    public class RouteTripService : IRouteTrip
    {
        private readonly AppDbContext _db;

        public RouteTripService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<ActionResult> CreateRouteTrip(RouteInfo info, string userId, CancellationToken cancellationToken)
        {
            try
            {
                var driver = _db.Drivers.FirstOrDefault(d => d.UserId == userId);
                var route = await _db.Routes.
                    FirstAsync(r => r.StartCityId == info.StartCityId &&
                                    r.FinishCityId == info.FinishCityId, cancellationToken);
                var routeDate = new RouteDate
                {
                    Route = route,
                    CreateDateTime = info.TripTime
                };
                var trip = new RouteTrip
                {
                    RouteDate = routeDate,
                    Driver = driver
                };
                await _db.RouteTrips.AddAsync(trip, cancellationToken);
                await _db.RouteDate.AddAsync(routeDate, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                return new OkObjectResult(trip);
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}