using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverService
{
    public class RouteTripService : IRouteTrip
    {
        private readonly AppDbContext _db;
        private readonly IDriver _driver;
        
        public RouteTripService(AppDbContext db, IDriver driver)
        {
            _db = db;
            _driver = driver;
        }

        public async Task<ActionResult> CreateAsync(RouteInfo info, string userId, CancellationToken cancellationToken)
        {
            var driver = await _db.Drivers.FirstAsync(d => d.UserId == userId, cancellationToken);
            var anyRoute = await _db.RouteTrips.AnyAsync(r => r.Driver.Id == driver.Id && r.IsActive, cancellationToken);
            if (anyRoute)
            {
                return new BadRequestObjectResult("Сначала завершите текущий маршрут");
            }
            await CreateRouteTrip(info, driver, cancellationToken);
          //  return new OkObjectResult(await _driver.FindClientPackagesAsync(userId));
            return new OkObjectResult(info);
        }

        private async Task CreateRouteTrip(RouteInfo info, Driver driver, CancellationToken cancellationToken)
        {
            var route = await _db.Routes.FirstAsync(r =>
                r.StartCityId == info.StartCity.Id &&
                r.FinishCityId == info.FinishCity.Id, cancellationToken);
            var trip = new RouteTrip
            {
                Driver = driver,
                Route = route
            };
            await _db.RouteTrips.AddAsync(trip, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
        
    }
}
