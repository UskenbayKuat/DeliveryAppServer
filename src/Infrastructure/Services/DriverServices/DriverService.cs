using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.DriverInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverServices
{
    public class DriverService : IDriver
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;        
        private readonly ContextHelper _contextHelper;


        public DriverService(AppDbContext db, AppIdentityDbContext identityDbContext, IMapper mapper,ContextHelper contextHelper)
        {
            _db = db;
            _identityDbContext = identityDbContext;
            _mapper = mapper;
            _contextHelper = contextHelper;
        }

        public async Task<string> FindDriverConnectionIdAsync(OrderInfo orderInfo,
            CancellationToken cancellationToken)
        {
            var routeTripList = await _contextHelper.Trips()
                .Where(r => r.Route.StartCity.Id == orderInfo.StartCity.Id 
                            && r.Route.FinishCity.Id == orderInfo.FinishCity.Id
                            && r.DeliveryDate.Day >= orderInfo.DeliveryDate.Day)
                .ToListAsync(cancellationToken);
            var order = await _contextHelper.Orders(o => o.Id == orderInfo.OrderId).FirstOrDefaultAsync(cancellationToken);
            foreach (var routeTrip in routeTripList)
            {
                var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == routeTrip.Driver.UserId, cancellationToken);
                if (string.IsNullOrEmpty(chatHub?.ConnectionId)) continue;
                if (await CheckRejectedAsync(routeTrip.Id, orderInfo.OrderId)) continue;
                await UpDateOrderStateAsync(routeTrip, order, GeneralState.OnReview);
                return chatHub.ConnectionId;
            }
            order.Delivery?.Orders.Remove(order);
            order.State = await _contextHelper.FindStateAsync((int)GeneralState.Waiting);
            _db.Orders.Update(order);
            await _db.SaveChangesAsync(cancellationToken);
            return string.Empty;
        }

        public async Task<List<OrderInfo>> FindOrdersAsync(string driverUserId)
        {
            var ordersInfo = new List<OrderInfo>();
            var routeTrip = await _contextHelper.Trip(driverUserId) ?? throw new HubException();
            var state = await _contextHelper.FindStateAsync((int)GeneralState.Waiting);
            var waitingOrders = await _contextHelper.Orders(o =>
                o.Route.Id == routeTrip.Route.Id &&
                o.DeliveryDate.Day <= routeTrip.DeliveryDate.Day && 
                o.State == state).ToListAsync();
            foreach(var waitingOrder in waitingOrders)
            {
                await UpDateOrderStateAsync(routeTrip, waitingOrder, GeneralState.OnReview);
                var user = await _identityDbContext.Users.FirstOrDefaultAsync(u =>u.Id == waitingOrder.Client.UserId);
                ordersInfo.Add(_mapper.Map<OrderInfo>(waitingOrder).SetClientData(user.Name, user.Surname, user.PhoneNumber));
            }
            return ordersInfo;
        }
        
        public async Task<ActionResult> GetOnReviewOrdersForDriverAsync(string driverUserId)
        {
            try
            {
                var routeTrip = await _contextHelper.Trip(driverUserId) ?? throw new NullReferenceException("Для проверки заказов создайте поездку");
                var state = await _contextHelper.FindStateAsync((int)GeneralState.OnReview);
                var ordersInfo = new List<OrderInfo>();
                await _contextHelper.Orders(o => o.Delivery.RouteTrip.Id == routeTrip.Id && o.State.Id == state.Id)
                    .ForEachAsync( o =>
                    {
                        var userClient = _identityDbContext.Users.First(u => u.Id == o.Client.UserId);
                        ordersInfo.Add(_mapper.Map<OrderInfo>(o).SetClientData(userClient.Name, userClient.Surname, userClient.PhoneNumber));
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
                await _contextHelper.Orders(o => 
                        o.Delivery.RouteTrip.Id == routeTrip.Id &&
                        (o.State.Id == stateInProgress.Id || o.State.Id == stateHandOver.Id || o.State.Id == stateReceived.Id))
                    .ForEachAsync( o =>
                    {
                        var userClient = _identityDbContext.Users.First(u => u.Id == o.Client.UserId);
                        ordersInfo.Add(_mapper.Map<OrderInfo>(o).SetClientData(userClient.Name, userClient.Surname, userClient.PhoneNumber));
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
                var order = await _contextHelper.Orders(o => o.Id == orderInfo.OrderId).FirstOrDefaultAsync();
                var routeTrip = await _contextHelper.Trip(driverUserId);
                await _db.RejectedOrders
                    .AddAsync(new RejectedOrder
                    {
                        Order = order, 
                        RouteTrip = routeTrip
                    });
                await _db.SaveChangesAsync();
                var driverConnectionId = await FindDriverConnectionIdAsync(orderInfo, default);
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

        private async Task UpDateOrderStateAsync(RouteTrip routeTrip, Order order, GeneralState generalState)
        {
            order.State = await _contextHelper.FindStateAsync((int)generalState);
            var delivery = await _db.Deliveries.FirstOrDefaultAsync(d => d.RouteTrip.Id == routeTrip.Id);
            delivery.Orders.Add(order);
            _db.Deliveries.Update(delivery);
            await _db.SaveChangesAsync();
        }
        

    }
}