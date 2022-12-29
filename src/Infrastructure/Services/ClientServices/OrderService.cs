using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientServices
{
    public class OrderService : IOrder
    {
        private readonly AppIdentityDbContext _dbIdentityDbContext;
        private readonly IDriver _driverService;
        private readonly IContext _context;

        public OrderService(AppIdentityDbContext dbIdentityDbContext, IDriver driverService, IContext context)
        {
            _dbIdentityDbContext = dbIdentityDbContext;
            _driverService = driverService;
            _context = context;
        }

        public async Task<ActionResult> CreateAsync(OrderInfo info, string clientUserId, Func<string, Task> func,
            CancellationToken cancellationToken)
        {
            try
            {
                var client = await _context.FindAsync<Client>(c => c.UserId == clientUserId);
                var carType = await _context.FindAsync<CarType>(c => c.Id == info.CarType.Id);
                var route = await _context.FindAsync<Route>(r => 
                    r.StartCityId == info.StartCity.Id && 
                    r.FinishCityId == info.FinishCity.Id);
                var state = await _context.FindAsync<State>((int)GeneralState.New);
                var order = new Order(info.IsSingle, info.Price, info.DeliveryDate)
                {
                    Client = client,
                    Package = info.Package,
                    CarType = carType,  
                    Route = route,
                    State =  state,
                    Location = new Location(info.Location.Latitude, info.Location.Longitude)
                };
                await _context.AddAsync(order);
                var driverConnectionId = await _driverService.FindDriverConnectionIdAsync(order, cancellationToken);
                await func(driverConnectionId);
                return new OkObjectResult(info);
            }
            catch
            {
                return new BadRequestResult();
            }
        }
        
        public async Task<List<OrderInfo>> FindWaitingOrdersAsync(string driverUserId)
        {
            var ordersInfo = new List<OrderInfo>();
            var delivery = await _context.Deliveries()
                .IncludeRouteTripAndRouteBuilder()
                .FirstOrDefaultAsync(d => d.RouteTrip.Driver.UserId == driverUserId); // check debug Driver.UserId is null ? (Include(Driver)) : delete comment
            var state = await _context.FindAsync<State>((int)GeneralState.Waiting);
            var waitingOrders = await _context.Orders().IncludeOrdersInfoBuilder().Where(o =>
                o.Route.Id == delivery.RouteTrip.Route.Id &&
                o.DeliveryDate.Day <= delivery.RouteTrip.DeliveryDate.Day && 
                o.State == state).ToListAsync();
            foreach(var waitingOrder in waitingOrders)
            {
                waitingOrder.State = await _context.FindAsync<State>((int)GeneralState.OnReview);
                delivery.AddOrder(waitingOrder);
                var userClient = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u =>u.Id == waitingOrder.Client.UserId);
                ordersInfo.Add(waitingOrder.GetOrderInfo(userClient));
            }
            await _context.UpdateAsync(delivery);
            return ordersInfo;
        }
        

        public async Task<ActionResult> GetWaitingOrdersAsync(string clientUserId,CancellationToken cancellationToken)
        {
            return new OkObjectResult(await GetOrderInfoAsync(clientUserId, GeneralState.Waiting));
        }

        public async Task<ActionResult> GetOnReviewOrdersAsync(string clientUserId, CancellationToken cancellationToken)
        {
            return new OkObjectResult(await GetOrderInfoAsync(clientUserId, GeneralState.OnReview));
        }

        private async Task<List<OrderInfo>> GetOrderInfoAsync(string clientUserId, GeneralState status)
        {
            var ordersInfo = new List<OrderInfo>();
            var userClient = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId);
            var state = await _context.FindAsync<State>((int)status);
            await _context.Orders()
                .IncludeOrdersInfoBuilder()
                .Where(o => o.Client.UserId == clientUserId && o.State == state)
                .ForEachAsync(o => ordersInfo.Add(o.GetOrderInfo(userClient)));
            return ordersInfo;
        }
        
        
    }
}