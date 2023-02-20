using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DeliveryServices
{
    public class DeliveryCommand : IDeliveryCommand
    {
        private readonly IContext _context;
        private readonly IChatHub _chatHub;
        private readonly IOrderContextBuilder _orderContextBuilder;
        private readonly IDeliveryContextBuilder _deliveryContextBuilder;
        private readonly IDriverContextBuilder _driverContextBuilder;

        public DeliveryCommand(IContext context, IChatHub chatHub, IOrderContextBuilder orderContextBuilder, IDeliveryContextBuilder deliveryContextBuilder, IDriverContextBuilder driverContextBuilder)
        {
            _context = context;
            _chatHub = chatHub;
            _orderContextBuilder = orderContextBuilder;
            _deliveryContextBuilder = deliveryContextBuilder;
            _driverContextBuilder = driverContextBuilder;
        }
        public async Task<Delivery> CreateAsync(RouteTripInfo tripInfo, string userId)
        {
            var driver = await _driverContextBuilder.CarBuilder()
                .Build()
                .FirstOrDefaultAsync(d => d.UserId == userId);
            if (driver?.Car is null) throw new CarNotExistsException();
            return await _deliveryContextBuilder.Build()
                .AnyAsync(d =>
                    d.Driver.Id == driver.Id && (
                        d.State.Id == (int)GeneralState.New ||
                        d.State.Id == (int)GeneralState.InProgress))
                ? throw new NotSupportedException()
                : await CreateDeliveryAsync(tripInfo, driver);
        }
        
        
        public async Task<Order> AddOrderAsync(int orderId)
        {
            var order = await _orderContextBuilder
                .ClientBuilder()
                .Build()
                .FirstAsync(c => c.Id == orderId);
            order.State = await _context.FindAsync<State>((int)GeneralState.PendingForHandOver);
            await _context.UpdateAsync(order.SetSecretCode());
            return order;
        }
        public async Task<Delivery> FindIsNewDelivery(Order order, CancellationToken cancellationToken)
        {
            var deliveries = await DeliveriesStateFromNewAsync(order, cancellationToken);
            foreach (var delivery in deliveries)
            {
                if (await CheckRejectedAsync(delivery, order)) continue;
                var connectionId = await _chatHub.GetConnectionIdAsync(delivery.Driver.UserId, cancellationToken);
                if (!string.IsNullOrEmpty(connectionId)) return delivery;
            }
            return default;
        }
        
        private async Task<Delivery> CreateDeliveryAsync(RouteTripInfo tripInfo, Driver driver)
        {
            var route = await _context.FindAsync<Route>(r =>
                r.StartCityId == tripInfo.StartCity.Id &&
                r.FinishCityId == tripInfo.FinishCity.Id);
            var state = await _context.FindAsync<State>((int)GeneralState.New);
            var delivery = new Delivery(tripInfo.DeliveryDate)
            {
                State = state,
                Driver = driver,
                Route = route
            };
            await _context.AddAsync(
                new LocationData
                {
                    Location = new Location(tripInfo.Location.Latitude, tripInfo.Location.Longitude),
                    Delivery = delivery
                });
            return delivery;
        }

        private async Task<List<Delivery>> DeliveriesStateFromNewAsync(Order order, CancellationToken cancellationToken) =>
            await _deliveryContextBuilder
                .DriverBuilder()
                .Build()
                .Where(d =>
                    d.Route.Id == order.Route.Id &&
                    d.DeliveryDate >= order.DeliveryDate &&
                    d.State.Id == (int)GeneralState.New)
                .ToListAsync(cancellationToken);

        private async Task<bool> CheckRejectedAsync(Delivery delivery, Order order) =>
            await _context
                .AnyAsync<RejectedOrder>(r =>
                    r.Delivery.Id == delivery.Id &&
                    r.Order.Id == order.Id);

    }
}