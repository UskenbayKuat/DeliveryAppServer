using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.RejectedInterfaces;
using ApplicationCore.Interfaces.RouteInterfaces;
using ApplicationCore.Interfaces.StateInterfaces;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Entities.Locations;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values;
using ApplicationCore.Models.Values.Enums;
using ApplicationCore.Specifications;
using ApplicationCore.Specifications.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public OrderCommand(
            IAsyncRepository<Order> context, 
            IBackgroundTaskQueue backgroundTask, 
            IState state, 
            IRejected rejected, IClient client, IAsyncRepository<CarType> contextCarType, IRoute route)
        {
            _context = context;
            _backgroundTask = backgroundTask;
            _state = state;
            _rejected = rejected;
            _client = client;
            _contextCarType = contextCarType;
            _route = route;
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
            order.State = await _state.GetByStateAsync(GeneralState.ReceivedByDriver);
            await _context.UpdateAsync(order);
        }

        public async Task<Order> RejectAsync(int orderId)
        {
            var orderSpec = new OrderForRejectSpecification(orderId);
            var order = await _context.FirstOrDefaultAsync(orderSpec);
            await _rejected.AddAsync(order);
            order.Delivery = default;
            order.State = await _state.GetByStateAsync(GeneralState.WaitingOnReview);
            await _context.UpdateAsync(order);
            return order;
        }
    
        public async Task SetDeliveryAsync(Order order, Delivery delivery)
        {
            order.State = await _state.GetByStateAsync(GeneralState.OnReview);
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
            var order = await _context.FirstOrDefaultAsync(orderSpec);
            order.State = await _state.GetByStateAsync(GeneralState.PendingForHandOver);
            await _context.UpdateAsync(order.SetSecretCode());
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetWaitingOrders(int routeId, DateTime dateTime)
        {
            var orderSpec = new OrderWithStateSpecification(routeId, dateTime);
            return await _context.ListAsync(orderSpec);
        }
    }
}