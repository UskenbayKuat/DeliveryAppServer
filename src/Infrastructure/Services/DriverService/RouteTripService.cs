using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Http;
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
            var driver = await _db.Drivers.FirstAsync(d => d.UserId == userId, cancellationToken)
                         ?? throw new BadHttpRequestException($"Такого пользоватея не существует");
            var result =
                await _db.RouteTrips.Include(r => r.Driver)
                    .AnyAsync(r => r.Driver == driver && r.IsActive == true, cancellationToken);
            if (result)
            {
                return new BadRequestObjectResult("Сначала завершите текущий маршрут");
            }
            var route = await _db.Routes.FirstAsync(r =>
                r.StartCityId == info.StartCityId &&
                r.FinishCityId == info.FinishCityId, cancellationToken);
            var trip = new RouteTrip
                { Driver = driver, RouteDate = new RouteDate(info.TripTime) { Route = route } };

            await _db.RouteTrips.AddAsync(trip, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(trip);
        }
    }
}
