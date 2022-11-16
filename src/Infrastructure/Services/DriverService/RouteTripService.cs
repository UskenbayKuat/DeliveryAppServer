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
                var driver = await _db.Drivers.FirstAsync(d => d.UserId == userId, cancellationToken);
                var route = await _db.Routes.FirstAsync(r => 
                                                                r.StartCityId == info.StartCityId &&
                                                                r.FinishCityId == info.FinishCityId, cancellationToken);
                var trip = new RouteTrip().AddRouteTripData(driver, new RouteDate(info.TripTime).AddRoute(route));
                
                await _db.RouteTrips.AddAsync(trip, cancellationToken);
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