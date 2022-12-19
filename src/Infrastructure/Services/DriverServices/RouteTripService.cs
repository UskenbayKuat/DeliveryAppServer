using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverServices
{
    public class RouteTripService : IRouteTrip
    {
        private readonly AppDbContext _db;
        private readonly IDriver _driver;
        private readonly IOrder _order;
        private readonly ContextHelper _contextHelper;
        private readonly IMapper _mapper;
        
        public RouteTripService(AppDbContext db, IDriver driver, ContextHelper contextHelper, IMapper mapper, IOrder order)
        {
            _db = db;
            _driver = driver;
            _contextHelper = contextHelper;
            _mapper = mapper;
            _order = order;
        }

        public async Task<ActionResult> CreateAsync(RouteTripInfo tripInfo, string userId, Func<string, bool, Task> func)
        {
            var driver = await _db.Drivers.FirstAsync(d => d.UserId == userId);
            var anyRoute = await _db.RouteTrips.AnyAsync(r => r.Driver.Id == driver.Id && r.IsActive);
            if (anyRoute)
            {
                return new BadRequestObjectResult("Сначала завершите текущий маршрут");
            }
            await CreateRouteTripAsync(tripInfo, driver);
            var driverChatHub = await _db.ChatHubs.FirstAsync(c => c.UserId == userId);
            var ordersInfo = await _order.FindWaitingOrdersAsync(userId);
            await func(driverChatHub?.ConnectionId, ordersInfo.Any());
            return new OkObjectResult(tripInfo);
        }
        
        public async Task<ActionResult> GetRouteTripIsActiveAsync(string driverUserId)
        {
            try
            {
                var routeTrip = await _contextHelper.Trip(driverUserId) ?? throw new NullReferenceException("Текущих поездок нет");
                return new OkObjectResult(_mapper.Map<RouteTripInfo>(routeTrip));
            }
            catch (NullReferenceException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch
            {
                return new BadRequestObjectResult("Non correct");
            }
        }
        

        private async Task CreateRouteTripAsync(RouteTripInfo tripInfo, Driver driver)
        {
            var route = await _contextHelper.FindRouteAsync(tripInfo.StartCity.Id, tripInfo.FinishCity.Id);
            var state = await _contextHelper.FindStateAsync((int)GeneralState.New);
            var trip = new RouteTrip(tripInfo.DeliveryDate)
            {
                Driver = driver,
                Route = route
            };
            var delivery = new Delivery
            {
                State = state,
                RouteTrip = trip

            };
            var locationDate = new LocationDate
            {
                Location = new Location(tripInfo.Location.Latitude, tripInfo.Location.Longitude),
                RouteTrip = trip
            };
            await _db.LocationDate.AddAsync(locationDate);
            await _db.Deliveries.AddAsync(delivery);
            await _db.SaveChangesAsync();
        }
        
    }
}
