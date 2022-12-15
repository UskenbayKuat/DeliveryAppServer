using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
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

namespace Infrastructure.Services.DriverService
{
    public class DriverService : IDriver
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;        
        private readonly StateHelper _stateHelper;


        public DriverService(AppDbContext db, AppIdentityDbContext identityDbContext, IMapper mapper,StateHelper stateHelper)
        {
            _db = db;
            _identityDbContext = identityDbContext;
            _mapper = mapper;
            _stateHelper = stateHelper;
        }

        public async Task<string> FindDriverConnectionIdAsync(OrderInfo orderInfo,
            CancellationToken cancellationToken)
        {
            var routeTripList = await Trips()
                .Where(r => r.Route.StartCity.Id == orderInfo.StartCity.Id 
                            && r.Route.FinishCity.Id == orderInfo.FinishCity.Id
                            && r.DeliveryDate.Day >= orderInfo.DeliveryDate.Day)
                .ToListAsync(cancellationToken);
            var order = await OrderAsync(orderInfo.OrderId, cancellationToken);
            foreach (var routeTrip in routeTripList)
            {
                var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == routeTrip.Driver.UserId, cancellationToken);
                if (string.IsNullOrEmpty(chatHub?.ConnectionId)) continue;
                if (await CheckRejectedAsync(routeTrip.Id, orderInfo.OrderId)) continue;
                await UpDateOrderStateAsync(routeTrip, order, GeneralState.OnReview);
                return chatHub.ConnectionId;
            }

            order.State =  _stateHelper.FindState((int)GeneralState.Waiting);
            _db.Orders.Update(order);
            await _db.SaveChangesAsync(cancellationToken);
            return string.Empty;
        }

        public async Task<List<OrderInfo>> FindClientPackagesAsync(string driverUserId)
        {
            var ordersInfo = new List<OrderInfo>();
            var routeTrip = await Trip(driverUserId) ?? throw new HubException();
            var waitingOrders = await WaitingOrders(routeTrip.Route.Id, routeTrip.DeliveryDate)
                .Where(o=>o.State == _stateHelper.FindState((int)GeneralState.Waiting)).ToListAsync();
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
                var routeTrip = await Trip(driverUserId) ?? throw new NullReferenceException("Для проверки заказов создайте поездку");
                var ordersInfo = new List<OrderInfo>();
                await _db.Orders
                    .Include(cp=>cp.Route.StartCity)
                    .Include(cp=>cp.Route.FinishCity)
                    .Include(cp=>cp.Package)
                    .Include(c => c.Client)
                    .Where(o => o.Delivery.RouteTrip.Id == routeTrip.Id && o.State == _stateHelper.FindState((int)GeneralState.OnReview))
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

        public async Task<ActionResult> GetRouteTripIsActiveAsync(string driverUserId)
        {
            try
            {
                var routeTrip = await Trip(driverUserId) ?? throw new NullReferenceException("Текущих поездок нет");
                return new OkObjectResult(_mapper.Map<RouteTripInfo>(routeTrip));
            }
            catch (NullReferenceException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<ActionResult> RejectNextFindDriverAsync(string driverUserId, OrderInfo orderInfo,Func<string, OrderInfo, Task> func)
        {
            try
            {
                var clientPackage = await OrderAsync(orderInfo.OrderId, default);
                var routeTrip = await Trip(driverUserId);
                await _db.RejectedOrders
                    .AddAsync(new RejectedOrder
                    {
                        Order = clientPackage, 
                        RouteTrip = routeTrip
                    });
                await _db.SaveChangesAsync();
                var driverConnectionId = await FindDriverConnectionIdAsync(orderInfo, default);
                await func(driverConnectionId, orderInfo);
                return new OkResult();
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
            order.State =  _stateHelper.FindState((int)generalState);
            var delivery = await _db.Deliveries.FirstOrDefaultAsync(d => d.RouteTrip.Id == routeTrip.Id);
            delivery.Orders.Add(order);
            _db.Deliveries.Update(delivery);
            await _db.SaveChangesAsync();
        }

        private async Task<Order> OrderAsync(int clientPackageId, CancellationToken cancellationToken) =>  await _db.Orders
            .Include(cp=>cp.Route.StartCity)
            .Include(cp=>cp.Route.FinishCity)
            .Include(cp=>cp.Package)
            .Include(c => c.Client)
            .FirstAsync(c => c.Id == clientPackageId, cancellationToken);
        
        private IQueryable<RouteTrip> Trips() => _db.RouteTrips
            .Include(r => r.Driver)
            .Include(r => r.Route.StartCity)
            .Include(r => r.Route.FinishCity)
            .Where(r => r.IsActive);

        private Task<RouteTrip> Trip(string userDriverId) => 
            Trips().FirstOrDefaultAsync(r => r.Driver.UserId == userDriverId);

        private IQueryable<Order> WaitingOrders(int id, DateTime deliveryDate) => _db.Orders
            .Include(o => o.Client)
            .Include(o => o.Package)
            .Include(o => o.Route.StartCity)
            .Include(o => o.Route.FinishCity)
            .Where(o =>
                o.Route.Id == id &&
                o.DeliveryDate.Day <= deliveryDate.Day);
        
    }
}