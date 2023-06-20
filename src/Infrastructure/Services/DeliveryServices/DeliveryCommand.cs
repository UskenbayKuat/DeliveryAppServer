using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Interfaces.RouteInterfaces;
using ApplicationCore.Interfaces.StateInterfaces;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values.Enums;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DeliveryServices
{
    public class DeliveryCommand : IDeliveryCommand
    {
        private readonly IAsyncRepository<Delivery> _context;
        private readonly IBackgroundTaskQueue _backgroundTask;
        private readonly IChatHub _chatHub;
        private readonly IDriver _driver;
        private readonly IState _state;
        private readonly IRoute _route;
        private readonly IOrderQuery _orderQuery;
        private readonly IOrderContextBuilder _orderContextBuilder;
        public DeliveryCommand(
            IAsyncRepository<Delivery> context,
            IBackgroundTaskQueue backgroundTask,
            IChatHub chatHub, 
            IRoute route, 
            IState state, 
            IDriver driver, 
            IOrderQuery orderQuery)
        {
            _chatHub = chatHub;
            _context = context;
            _route = route;
            _state = state;
            _driver = driver;
            _orderQuery = orderQuery;
            _backgroundTask = backgroundTask;
        }
        public async Task<Delivery> CreateAsync(CreateDeliveryDto dto)
        {
            var driver = await _driver.GetByUserIdAsync(dto.UserId);
            if (driver?.Car is null)
            {
                throw new CarNotExistsException();
            }
            var deliverySpec = new DeliveryWithStateSpecification(driver.Id);
            return await _context.AnyAsync(deliverySpec)
                ? throw new ArgumentException("У вас уже есть поездка")
                : await CreateDeliveryAsync(dto, driver);
        }

        public async Task<IReadOnlyList<Order>> AddWaitingOrderAsync(Delivery delivery)
        {
            var orders = await _orderQuery.GetWaitingOrders(delivery.Route.Id, delivery.DeliveryDate);
            var stateOnReview = await _state.GetByStateAsync(GeneralState.OnReview);
            foreach (var order in orders)
            {
                order.State = stateOnReview;
                delivery.AddOrder(order);
                await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, delivery.Id));
            }
            await _context.UpdateAsync(delivery);
            return orders;
        }

        public async Task<ActionResult> CancellationAsync(string driverUserId)
        {
            var deliverySpec = new DeliveryWithStateSpecification(driverUserId);
            var delivery = await _context.FirstOrDefaultAsync(deliverySpec);
            if (delivery.Orders.Count > 0)
            {
                throw new ArgumentException("У вас активные заказы");
            }
            delivery.State = await _state.GetByStateAsync(GeneralState.Canceled);
            await _context.UpdateAsync(delivery.SetCancellationDate());
            return new NoContentResult();
        }
        
        public async Task<Order> AddOrderAsync(int orderId)
        {
            var orderSpec = new OrderWithClientSpecification(orderId);
            var order = await _orderContextBuilder
                .ClientBuilder()
                .Build()
                .FirstAsync(c => c.Id == orderId);
            order.State = await _context.FindAsync<State>((int)GeneralState.PendingForHandOver);
            await _context.UpdateAsync(order.SetSecretCode());
            return order;
        }

        public async Task StartAsync(string driverUserId)
        {
            var delivery = await _context.FindAsync<Delivery>(d => d.Driver.UserId == driverUserId && d.State.Id == (int)GeneralState.New);
            if (delivery != null)
            {
                delivery.State = await _context.FindAsync<State>(s => s.Id == (int)GeneralState.InProgress);
                await _context.UpdateAsync(delivery);
            }
        }

        public async Task<Delivery> FindIsNewDelivery(Order order)
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
        
        private async Task<Delivery> CreateDeliveryAsync(CreateDeliveryDto dto, Driver driver)
        {
            var route = await _route.GetByCitiesIdAsync(dto.StartCityId, dto.FinishCityId);
            var state = await _state.GetByStateAsync(GeneralState.Waiting);
            var delivery = new Delivery(dto.DeliveryDate)
            {
                State = state,
                Driver = driver,
                Route = route,
                Location = dto.Location
            };
            await _context.AddAsync(delivery);
            return delivery;
        }

        private async Task<List<Delivery>> DeliveriesStateFromNewAsync(Order order, CancellationToken cancellationToken) =>
            await _deliveryContextBuilder
                .DriverBuilder()
                .Build()
                .OrderBy(d => Math.Abs(d.Location.Latitude - order.Location.Latitude) + Math.Abs(d.Location.Longitude - order.Location.Longitude))
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