using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientServices
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _dbIdentityDbContext;
        private readonly ContextHelper _contextHelper;
        private readonly IDriver _driverService;

        public OrderService(AppDbContext db, AppIdentityDbContext dbIdentityDbContext,ContextHelper contextHelper, IDriver driverService)
        {
            _db = db;
            _dbIdentityDbContext = dbIdentityDbContext;
            _contextHelper = contextHelper;
            _driverService = driverService;
        }

        public async Task<ActionResult> CreateAsync(OrderInfo info, string clientUserId, Func<string, Task> func,
            CancellationToken cancellationToken)
        {
            try
            {
                var client = await _db.Clients.FirstAsync(c => c.UserId == clientUserId, cancellationToken);
                var carType = await _db.CarTypes.FirstAsync(c => c.Id == info.CarType.Id, cancellationToken);
                var route = await _contextHelper.FindRouteAsync(info.StartCity.Id, info.FinishCity.Id);
                var state = await _contextHelper.FindStateAsync((int)GeneralState.New);
                var order = new Order(info.IsSingle, info.Price, info.DeliveryDate)
                {
                    Client = client,
                    Package = info.Package,
                    CarType = carType,  
                    Route = route,
                    State =  state,
                    Location = new Location(info.Location.Latitude, info.Location.Longitude)
                };
                await _db.Orders.AddAsync(order, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
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
            var routeTrip = await _contextHelper.Trip(driverUserId);
            var state = await _contextHelper.FindStateAsync((int)GeneralState.Waiting);
            var waitingOrders = await _contextHelper.OrdersForOrderInfo().Where(o =>
                o.Route.Id == routeTrip.Route.Id &&
                o.DeliveryDate.Day <= routeTrip.DeliveryDate.Day && 
                o.State == state).ToListAsync();
            foreach(var waitingOrder in waitingOrders)
            {
                await _contextHelper.AddOrderToDeliveryAsync(routeTrip, waitingOrder, GeneralState.OnReview);
                var userClient = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u =>u.Id == waitingOrder.Client.UserId);
                ordersInfo.Add(waitingOrder.GetOrderInfo(userClient));
            }
            return ordersInfo;
        }
        

        public async Task<ActionResult> GetWaitingOrdersAsync(string clientUserId,CancellationToken cancellationToken)
        {
            var ordersInfo = new List<OrderInfo>();
            var userClient = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            var state = await _contextHelper.FindStateAsync((int) GeneralState.Waiting);
            await _contextHelper.OrdersForOrderInfo().Where(o => o.Client.UserId == clientUserId && o.State == state).
            ForEachAsync(o => ordersInfo.Add(o.GetOrderInfo(userClient)), cancellationToken);
            return new OkObjectResult(ordersInfo);
        }

        public async Task<ActionResult> GetOnReviewOrdersAsync(string clientUserId, CancellationToken cancellationToken)
        {
            var ordersInfo = new List<OrderInfo>();
            var userClient = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            var state = await _contextHelper.FindStateAsync((int)GeneralState.OnReview);
            await _contextHelper.OrdersForOrderInfo().Where(o => o.Client.UserId == clientUserId && o.State == state).
                ForEachAsync(o => ordersInfo.Add(o.GetOrderInfo(userClient)), cancellationToken);
            return new OkObjectResult(ordersInfo);
        }
    }
}