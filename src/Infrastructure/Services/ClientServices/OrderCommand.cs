using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.Histories;
using ApplicationCore.Interfaces.RejectedInterfaces;
using ApplicationCore.Interfaces.RouteInterfaces;
using ApplicationCore.Interfaces.StateInterfaces;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Locations;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using ApplicationCore.Specifications.Orders;

namespace Infrastructure.Services.ClientServices
{
    public class OrderCommand : IOrderCommand
    {
        private readonly IAsyncRepository<Order> _context;
        private readonly IAsyncRepository<CarType> _contextCarType;
        private readonly IBackgroundTaskQueue _backgroundTask;
        private readonly IState _state;
        private readonly IRejected _rejected;
        private readonly IClient _client;
        private readonly IRoute _route;
        private readonly IOrderStateHistory _stateHistory;
        public OrderCommand(
            IAsyncRepository<Order> context, 
            IBackgroundTaskQueue backgroundTask, 
            IState state, 
            IRejected rejected, 
            IClient client, 
            IAsyncRepository<CarType> contextCarType, 
            IRoute route, 
            IOrderStateHistory stateHistory)
        {
            _context = context;
            _backgroundTask = backgroundTask;
            _state = state;
            _rejected = rejected;
            _client = client;
            _contextCarType = contextCarType;
            _route = route;
            _stateHistory = stateHistory;
        }

        public async Task<Order> CreateAsync(CreateOrderDto dto, string clientUserId)
        {
            var client = await _client.GetByUserId(clientUserId);
            var carType = await _contextCarType.FirstOrDefaultAsync(c => c.Name == dto.CarTypeName);
            var route = await _route.GetByCitiesNameAsync(dto.StartCityName, dto.FinishCityName);
            var state = await _state.GetByStateAsync(GeneralState.WaitingOnReview);
            var order = new Order(dto.IsSingle, dto.Price, dto.DeliveryDate, dto.Description, dto.AddressTo, dto.AddressFrom)
            {
                Client = client,
                Package = dto.Package,
                CarType = carType,
                Route = route,
                State = state,
                Location = new Location(dto.Location.Latitude, dto.Location.Longitude)
            };
            return await _context.AddAsync(order);
        }

        public async Task ConfirmHandOverAsync(ConfirmHandOverDto dto)
        {
            var orderSpec = new OrderWithStateSpecification(dto.OrderId);
            var order = await _context.FirstOrDefaultAsync(orderSpec);
            if (order.State.StateValue != GeneralState.PendingForHandOver 
                || order.SecretCode != dto.SecretCode.ToUpper())
            {
                throw new ArgumentException("Не совпадает код");
            }
            var state = await _state.GetByStateAsync(GeneralState.ReceivedByDriver);
            order.State = state;
            await _stateHistory.AddAsync(order, state);
            await _context.UpdateAsync(order.SetSecretCodeEmpty());
        }

        public async Task<Order> RejectAsync(int orderId)
        {
            var orderSpec = new OrderForRejectSpecification(orderId);
            var order = await _context.FirstOrDefaultAsync(orderSpec);
            if (order == null)
            {
                return default;
            }
            await _stateHistory.RemoveAsync(order.Id, order.State.Id); 
            await _rejected.AddAsync(order);
            var state = await _state.GetByStateAsync(GeneralState.WaitingOnReview);
            order.Delivery = default;
            order.State = state;
            await _context.UpdateAsync(order.SetSecretCodeEmpty());
            return order;
        }
    
        public async Task SetDeliveryAsync(Order order, Delivery delivery)
        {
            var state = await _state.GetByStateAsync(GeneralState.OnReview);
            order.State = state;
            order.Delivery = delivery;
            await _context.UpdateAsync(order);
            await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, order.Delivery.Id));
        }

        public async Task<bool> IsOnReview(BackgroundOrder backgroundOrder)
        {
            return await _context
                .AnyAsync(o => 
                    o.Id == backgroundOrder.OrderId &&
                    o.Delivery.Id == backgroundOrder.DeliveryId && 
                    o.State.StateValue == GeneralState.OnReview);
        }

        public async Task<Order> UpdateStatePendingAsync(int orderId)
        {            
            var orderSpec = new OrderWithClientSpecification(orderId);
            var order = await _context.FirstOrDefaultAsync(orderSpec)
                    ?? throw new ArgumentException($"Нет такого заказа с Id: {orderId}");
            var state = await _state.GetByStateAsync(GeneralState.PendingForHandOver);
            order.State = state;
            await _stateHistory.AddAsync(order, state);
            await _context.UpdateAsync(order.SetSecretCode());
            return order;
        }

        public async Task CancelAsync(int orderId)
        {
            var spec = new OrderWithStateSpecification(orderId, GeneralState.WaitingOnReview);
            var order = await _context.FirstOrDefaultAsync(spec)
                ?? throw new ArgumentException($"Активный заказ нельзя отменить");
            await _context.DeleteAsync(order);
        }
    }
}