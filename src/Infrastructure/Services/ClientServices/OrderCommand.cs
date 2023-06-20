using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.StateInterfaces;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Entities.Locations;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values.Enums;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientServices
{
    public class OrderCommand : IOrderCommand
    {
        private readonly IAsyncRepository<Order> _context;
        private readonly IOrderContextBuilder _orderContextBuilder;
        private readonly IBackgroundTaskQueue _backgroundTask;
        private readonly IState _state;

        public OrderCommand(IAsyncRepository<Order> context, IOrderContextBuilder orderContextBuilder, IBackgroundTaskQueue backgroundTask, IState state)
        {
            _context = context;
            _orderContextBuilder = orderContextBuilder;
            _backgroundTask = backgroundTask;
            _state = state;
        }

        public async Task<Order> CreateAsync(CreateOrderDto dto, string clientUserId, CancellationToken cancellationToken)
        {
            var client = await _context.FindAsync<Client>(c => c.UserId == clientUserId);
            var carType = await _context.FindAsync<CarType>(c => c.Name == dto.CarTypeName);
            var route = await _context.FindAsync<Route>(r =>
                r.StartCity.Name == dto.StartCityName &&
                r.FinishCity.Name == dto.FinishCityName);
            var state = await _context.FindAsync<State>((int)GeneralState.Waiting);
            var order = new Order(dto.IsSingle, dto.Price, dto.DeliveryDate, dto.Description, dto.AddressTo, dto.AddressFrom)
            {
                Client = client,
                Package = dto.Package,
                CarType = carType,
                Route = route,
                State = state,
                Location = new Location(dto.Location.Latitude, dto.Location.Longitude)
            };
            await _context.AddAsync(order);
            return order;
        }

        public async Task<ActionResult> ConfirmHandOverAsync(ConfirmHandOverInfo info, CancellationToken cancellationToken)
        {
            var order = await _orderContextBuilder
                .StateBuilder()
                .Build()
                .FirstOrDefaultAsync(o => o.Id == info.OrderId, cancellationToken);
            if (order.State.Id != (int)GeneralState.PendingForHandOver || order.SecretCode != info.SecretCode.ToUpper()) //TODO проверка на deliveryId == order.Delivery.Id нужен?
            {
                return new BadRequestResult();
            }
            order.State = await _context.FindAsync<State>(s => s.Id == (int)GeneralState.ReceivedByDriver);
            await _context.UpdateAsync(order);
            return new NoContentResult();
        }

        public async Task<Order> RejectAsync(int orderId)
        {
            var order = await _orderContextBuilder.ForRejectBuilder()
                .Build()
                .FirstOrDefaultAsync(o => o.Id == orderId);
            await _context.AddAsync(new RejectedOrder
            {
                Order = order,
                Delivery = order.Delivery
            });
            order.Delivery = default;
            order.State = await _context.FindAsync<State>((int)GeneralState.Waiting);
            await _context.UpdateAsync(order);
            return order;
        }

        public async Task SetDeliveryAsync(Order order, Delivery delivery)
        {
            order.State = await _context.FindAsync<State>((int)GeneralState.OnReview);
            order.Delivery = delivery;
            await _context.UpdateAsync(order);
            await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, order.Delivery.Id));
        }

        public async Task<bool> IsOnReview(BackgroundOrder backgroundOrder)
        {
            var order = await _orderContextBuilder
                .ForRejectBuilder()
                .Build()
                .FirstOrDefaultAsync(o =>
                    o.Id == backgroundOrder.OrderId && 
                    o.Delivery.Id == backgroundOrder.DeliveryId);
            return order?.State.StateValue == GeneralState.OnReview && 
                   order.Delivery.Id == backgroundOrder.DeliveryId;
        }

        public async Task<List<Order>> AddWaitingOrdersToDeliveryAsync(Delivery delivery)
        {
            var stateOnReview = await _state.GetByStateAsync(GeneralState.OnReview);
            var orderSpec = new OrderWithStateSpecification(delivery.Route.Id, delivery.DeliveryDate);
            var orders = await _context.ListAsync(orderSpec);
            foreach (var order in orders)
            {
                order.State = stateOnReview;
                delivery.AddOrder(order);
                await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, delivery.Id));
            }
            await _context.UpdateAsync(delivery);
            return orders;
        }

        public async Task<IReadOnlyList<Order>> GetWaitingOrders(int routeId, DateTime dateTime)
        {
            var orderSpec = new OrderWithStateSpecification(routeId, dateTime);
            return await _context.ListAsync(orderSpec);
        }
    }
}