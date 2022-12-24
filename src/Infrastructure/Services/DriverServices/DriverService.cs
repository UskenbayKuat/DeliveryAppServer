using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverServices
{
    public class DriverService : IDriver
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly ContextHelper _contextHelper;


        public DriverService(AppDbContext db, AppIdentityDbContext identityDbContext, ContextHelper contextHelper)
        {
            _db = db;
            _identityDbContext = identityDbContext;
            _contextHelper = contextHelper;
        }

        public async Task<string> FindDriverConnectionIdAsync(Order order,
            CancellationToken cancellationToken)
        {
            var routeTripList = await _contextHelper.Trips()
                .Where(r => r.Route.StartCity.Id == order.Route.StartCity.Id 
                            && r.Route.FinishCity.Id == order.Route.FinishCity.Id
                            && r.DeliveryDate.Day >= order.DeliveryDate.Day)
                .ToListAsync(cancellationToken);
            foreach (var routeTrip in routeTripList)
            {
                var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == routeTrip.Driver.UserId, cancellationToken);
                if (string.IsNullOrEmpty(chatHub?.ConnectionId)) continue;
                if (await CheckRejectedAsync(routeTrip.Id, order.Id)) continue;
                await _contextHelper.AddOrderToDeliveryAsync(routeTrip, order, GeneralState.OnReview);
                return chatHub.ConnectionId;
            }
            order.Delivery?.Orders.Remove(order);
            order.State = await _contextHelper.FindStateAsync((int)GeneralState.Waiting);
            _db.Orders.Update(order);
            await _db.SaveChangesAsync(cancellationToken);
            return string.Empty;
        }


        
        public async Task<ActionResult> GetOnReviewOrdersForDriverAsync(string driverUserId)
        {
            try
            {
                var routeTrip = await _contextHelper.Trip(driverUserId) ?? throw new NullReferenceException("Для проверки заказов создайте поездку");
                var state = await _contextHelper.FindStateAsync((int)GeneralState.OnReview);
                var ordersInfo = new List<OrderInfo>();
                await _contextHelper.OrdersForOrderInfoWithDriver().Where(o => o.Delivery.RouteTrip.Id == routeTrip.Id && o.State.Id == state.Id)
                    .ForEachAsync( o =>
                    {
                        var userClient = _identityDbContext.Users.First(u => u.Id == o.Client.UserId);
                        ordersInfo.Add(o.GetOrderInfo(userClient));
                    });
                return new OkObjectResult(ordersInfo);
            }
            catch (NullReferenceException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        
        public async Task<ActionResult> GetActiveOrdersForDriverAsync(string driverUserId)
        {
            try
            {
                var routeTrip = await _contextHelper.Trip(driverUserId) ?? throw new NullReferenceException("Для проверки заказов создайте поездку");
                var stateInProgress = await _contextHelper.FindStateAsync((int)GeneralState.InProgress);
                var stateHandOver = await _contextHelper.FindStateAsync((int)GeneralState.PendingForHandOver);
                var stateReceived = await _contextHelper.FindStateAsync((int)GeneralState.ReceivedByDriver);
                var ordersInfo = new List<OrderInfo>();
                await _contextHelper.OrdersForOrderInfoWithDriver().Where(o => 
                        o.Delivery.RouteTrip.Id == routeTrip.Id &&
                        (o.State.Id == stateInProgress.Id || o.State.Id == stateHandOver.Id || o.State.Id == stateReceived.Id))
                    .ForEachAsync( o =>
                    {
                        var userClient = _identityDbContext.Users.First(u => u.Id == o.Client.UserId);
                        ordersInfo.Add(o.GetOrderInfo(userClient));
                    });
                return new OkObjectResult(ordersInfo);
            }
            catch (NullReferenceException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        public async Task<ActionResult> RejectNextFindDriverAsync(string driverUserId, OrderInfo orderInfo,Func<string, Task> func)
        {
            try
            {
                var order = await _contextHelper.OrdersForReject().Where(o => o.Id == orderInfo.OrderId).FirstOrDefaultAsync();
                var routeTrip = await _contextHelper.Trip(driverUserId);
                await _db.RejectedOrders
                    .AddAsync(new RejectedOrder
                    {
                        Order = order, 
                        RouteTrip = routeTrip
                    });
                await _db.SaveChangesAsync();
                var driverConnectionId = await FindDriverConnectionIdAsync(order, default);
                await func(driverConnectionId);
                return new OkObjectResult(new OrderInfo());
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        private async Task<bool> CheckRejectedAsync(int routeTripId, int orderId)
        => await _db.RejectedOrders
                .AnyAsync(r =>
                    r.RouteTrip.Id == routeTripId &&
                    r.Order.Id == orderId);


        

    }
}