using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverServices
{
    public class DriverService : IDriver
    {
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IContext _context;


        public DriverService(AppIdentityDbContext identityDbContext, IContext context)
        {
            _identityDbContext = identityDbContext;
            _context = context;
        }

        public async Task<string> FindDriverConnectionIdAsync(Order order,
            CancellationToken cancellationToken)
        {
            var deliveries = await _context
                .Deliveries()
                .IncludeRouteTripAndDriverBuilder()
                .Where(r =>
                    r.RouteTrip.Route.StartCity.Id == order.Route.StartCityId &&
                    r.RouteTrip.Route.FinishCity.Id == order.Route.FinishCityId &&
                    r.RouteTrip.DeliveryDate.Day >= order.DeliveryDate.Day)
                .ToListAsync(cancellationToken);
            foreach (var delivery in deliveries)
            {
                var chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == delivery.RouteTrip.Driver.UserId);
                if (string.IsNullOrEmpty(chatHub?.ConnectionId)) continue;
                if (await CheckRejectedAsync(delivery.RouteTrip.Id, order.Id)) continue;
                order.State = await _context.FindAsync<State>((int)GeneralState.OnReview);
                await _context.UpdateAsync(delivery.AddOrder(order));
                return chatHub.ConnectionId;
            }

            order.Delivery?.Orders.Remove(order);
            order.State = await _context.FindAsync<State>((int)GeneralState.Waiting);
            await _context.UpdateAsync(order);
            return string.Empty;
        }


        public async Task<ActionResult> GetOnReviewOrdersForDriverAsync(string driverUserId)
        {
            try
            {
                var delivery = await _context.FindAsync<Delivery>(d => d.RouteTrip.Driver.UserId == driverUserId)
                               ?? throw new NullReferenceException("Для проверки заказов создайте поездку");
                var state = await _context.FindAsync<State>((int)GeneralState.OnReview);
                var ordersInfo = new List<OrderInfo>();
                await _context.Orders()
                    .IncludeOrdersInfoBuilder()
                    .Where(o => o.Delivery.Id == delivery.Id && o.State.Id == state.Id)
                    .ForEachAsync(o =>
                    {
                        var userClient = _identityDbContext.Users.FirstOrDefault(u => u.Id == o.Client.UserId);
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
                var delivery = await _context.FindAsync<Delivery>(d => d.RouteTrip.Driver.UserId == driverUserId)
                               ?? throw new NullReferenceException("Для проверки заказов создайте поездку");
                var stateInProgress = await _context.FindAsync<State>((int)GeneralState.InProgress);
                var stateHandOver = await _context.FindAsync<State>((int)GeneralState.PendingForHandOver);
                var stateReceived = await _context.FindAsync<State>((int)GeneralState.ReceivedByDriver);
                var ordersInfo = new List<OrderInfo>();
                await _context.Orders()
                    .IncludeOrdersInfoBuilder()
                    .Where(o =>
                        o.Delivery.Id == delivery.Id &&
                        (o.State.Id == stateInProgress.Id || o.State.Id == stateHandOver.Id ||
                         o.State.Id == stateReceived.Id))
                    .ForEachAsync(o =>
                    {
                        var userClient = _identityDbContext.Users.FirstOrDefault(u => u.Id == o.Client.UserId);
                        ordersInfo.Add(o.GetOrderInfo(userClient));
                    });
                return new OkObjectResult(ordersInfo);
            }
            catch (NullReferenceException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        public async Task<Order> RejectNextFindDriverAsync(string driverUserId, int orderId)
        {
            var order = await _context.Orders().IncludeForRejectBuilder()
                .FirstOrDefaultAsync(o => o.Id == orderId);
            var routeTrip = await _context.FindAsync<RouteTrip>(r => r.Driver.UserId == driverUserId);
            await _context.AddAsync(new RejectedOrder
            {
                Order = order,
                RouteTrip = routeTrip
            });
            order.Delivery?.Orders.Remove(order);
            order.State = await _context.FindAsync<State>((int)GeneralState.Waiting);
            await _context.UpdateAsync(order);
            return order;
        }

        private async Task<bool> CheckRejectedAsync(int routeTripId, int orderId) =>
            await _context
                .AnyAsync<RejectedOrder>(r =>
                    r.RouteTrip.Id == routeTripId &&
                    r.Order.Id == orderId);
    }
}