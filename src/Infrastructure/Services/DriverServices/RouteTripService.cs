﻿using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverServices
{
    public class RouteTripService : IRouteTrip
    {
        private readonly IOrder _order;
        private readonly IContext _context;
        
        public RouteTripService(IOrder order, IContext context)
        {
            _order = order;
            _context = context;
        }

        public async Task<ActionResult> CreateAsync(RouteTripInfo tripInfo, string userId, Func<string, bool, Task> func)
        {
            var driver = await _context.FindAsync<Driver>(d => d.UserId == userId);
            var anyTrip = await _context.AnyAsync<RouteTrip>(r => r.Driver.Id == driver.Id && r.IsActive);
            if (anyTrip)
            {
                return new BadRequestObjectResult("Сначала завершите текущий маршрут");
            }
            await CreateRouteTripAsync(tripInfo, driver);
            var driverChatHub = await _context.FindAsync<ChatHub>(c => c.UserId == userId);
            var ordersInfo = await _order.FindWaitingOrdersAsync(userId);
            await func(driverChatHub?.ConnectionId, ordersInfo.Any());
            return new OkObjectResult(tripInfo);
        }
        
        public async Task<ActionResult> GetRouteTripIsActiveAsync(string driverUserId)
        {
            try
            {
                var routeTrip = await _context.RouteTrips().IncludeRoutesBuilder().FirstOrDefaultAsync(r => r.Driver.UserId ==driverUserId) 
                                ?? throw new NullReferenceException("Текущих поездок нет");
                return new OkObjectResult(routeTrip.GetRouteTripInfo());
            }
            catch (NullReferenceException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch
            {
                return new BadRequestObjectResult("Not correct");
            }
        }
        

        private async Task CreateRouteTripAsync(RouteTripInfo tripInfo, Driver driver)
        {
            var route = await _context.FindAsync<Route>(r => 
                r.StartCityId == tripInfo.StartCity.Id && 
                r.FinishCityId == tripInfo.FinishCity.Id);
            var state = await _context.FindAsync<State>((int)GeneralState.New);
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
            await _context.AddAsync(locationDate);
            await _context.AddAsync(delivery); // <- check only AddAsync(locationDate), if saving delivery in db delete this row
        }
        
    }
}
