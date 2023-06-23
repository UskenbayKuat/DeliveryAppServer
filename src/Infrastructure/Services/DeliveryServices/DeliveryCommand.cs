using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Interfaces.RejectedInterfaces;
using ApplicationCore.Interfaces.RouteInterfaces;
using ApplicationCore.Interfaces.StateInterfaces;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using ApplicationCore.Specifications.Deliveries;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IRejected _rejected;
        public DeliveryCommand(
            IAsyncRepository<Delivery> context,
            IBackgroundTaskQueue backgroundTask,
            IChatHub chatHub, 
            IRoute route, 
            IState state, 
            IDriver driver, 
            IOrderQuery orderQuery, IRejected rejected)
        {
            _chatHub = chatHub;
            _context = context;
            _route = route;
            _state = state;
            _driver = driver;
            _orderQuery = orderQuery;
            _rejected = rejected;
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

        public async Task<IReadOnlyList<Order>> AddWaitingOrdersAsync(Delivery delivery)
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

        public async Task StartAsync(string driverUserId)
        {
            var deliverySpec = new DeliveryWithStateSpecification(driverUserId, GeneralState.WaitingOrder);
            var delivery = await _context.FirstOrDefaultAsync(deliverySpec);
            if (delivery != null)
            {
                delivery.State = await _state.GetByStateAsync(GeneralState.InProgress);
                await _context.UpdateAsync(delivery);
            }
        }

        public async Task<LocationDto> UpdateLocationAsync(LocationDto request)
        {
            var deliverySpec = new DeliveryWithLocationSpecification(request.UserId);
            var delivery = await _context.FirstOrDefaultAsync(deliverySpec);
            if (request.Latitude != 0 && request.Longitude != 0)
            {
                delivery?.Location.UpdateLocation(request.Latitude, request.Longitude);
                await _context.UpdateAsync(delivery);
                return request;
            }
            request.Latitude = delivery.Location.Latitude;
            request.Longitude = delivery.Location.Longitude;
            return request;
        }

        public async Task<Delivery> FindIsNewDelivery(Order order)
        {
            var deliverySpec = new DeliveryWithDriverSpecification(order.Route.Id, order.DeliveryDate, order.Location);
            var deliveries = await _context.ListAsync(deliverySpec);
            foreach (var delivery in deliveries)
            {
                if (await _rejected.CheckRejectedAsync(delivery.Id, order.Id)) continue;
                var connectionId = await _chatHub.GetConnectionIdAsync(delivery.Driver.UserId, default);
                if (!string.IsNullOrEmpty(connectionId)) return delivery;
            }
            return default;
        }
        
        private async Task<Delivery> CreateDeliveryAsync(CreateDeliveryDto dto, Driver driver)
        {
            var route = await _route.GetByCitiesIdAsync(dto.StartCityId, dto.FinishCityId);
            var state = await _state.GetByStateAsync(GeneralState.WaitingOrder);
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
    }
}