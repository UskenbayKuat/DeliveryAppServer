using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.AppData.DataAccess;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverService
{
    public class RouteTripService : IRouteTrip
    {
        private readonly AppDbContext _db;
        private readonly IDriver _driver;
        private readonly StateHelper _stateHelper;
        
        public RouteTripService(AppDbContext db, IDriver driver, StateHelper stateHelper)
        {
            _db = db;
            _driver = driver;
            _stateHelper = stateHelper;
        }

        public async Task<ActionResult> CreateAsync(RouteTripInfo tripInfo, string userId, CancellationToken cancellationToken)
        {
            var driver = await _db.Drivers.FirstAsync(d => d.UserId == userId, cancellationToken);
            var anyRoute = await _db.RouteTrips.AnyAsync(r => r.Driver.Id == driver.Id && r.IsActive, cancellationToken);
            if (anyRoute)
            {
                return new BadRequestObjectResult("Сначала завершите текущий маршрут");
            }
            await CreateRouteTrip(tripInfo, driver, cancellationToken);
            //TODO  await _driver.FindClientPackagesAsync(userId);
            return new OkObjectResult(tripInfo);
        }

        private async Task CreateRouteTrip(RouteTripInfo tripInfo, Driver driver, CancellationToken cancellationToken)
        {
            var route = await _db.Routes.FirstAsync(r =>
                r.StartCityId == tripInfo.StartCity.Id &&
                r.FinishCityId == tripInfo.FinishCity.Id, cancellationToken);
            var state =  _stateHelper.FindState((int)GeneralState.New);
            var delivery = new Delivery
            {
                State = state,
                RouteTrip = new RouteTrip(tripInfo.DeliveryDate)
                {
                    Driver = driver,
                    Route = route
                }
            };
            await _db.Deliveries.AddAsync(delivery, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
        
    }
}
