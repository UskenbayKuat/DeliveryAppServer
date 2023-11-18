using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Locations;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using ApplicationCore.Specifications.Orders;

namespace Infrastructure.Services.Clients
{
    public class OrderCommand : IOrderCommand
    {
        private readonly IAsyncRepository<Order> _context;
        private readonly IAsyncRepository<CarType> _contextCarType;
        private readonly IBackgroundTaskQueue _backgroundTask;
        private readonly IState _state;
        private readonly IRejectedOrder _rejected;
        private readonly IClient _client;
        private readonly IRoute _route;
        private readonly IOrderStateHistory _stateHistory;
        public OrderCommand(
            IAsyncRepository<Order> context,
            IBackgroundTaskQueue backgroundTask,
            IState state,
            IRejectedOrder rejected,
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

        public async Task<Order> CreateAsync(CreateOrderDto dto, Guid clientUserId)
        {
            var client = await _client.GetByUserId(clientUserId);
            var carType = await _contextCarType.FirstOrDefaultAsync(c => c.Name == dto.CarTypeName);
            var route = await _route.GetByCitiesNameAsync(dto.StartCityName, dto.FinishCityName);
            var state = await _state.GetByStateAsync(GeneralState.WAITING_ON_REVIEW);
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

        public async Task<Guid> QRCodeAcceptAsync(QRCodeDto dto)
        {
            var orderSpec = new OrderWithStateSpecification(dto.OrderId, dto.UserId);
            var order = await _context.FirstOrDefaultAsync(orderSpec);
            await order.CheckSecretCodeAsync(dto.SecretCode);
            order.State = order.State.StateValue switch
            {
                GeneralState.PENDING_For_HAND_OVER => await _state.GetByStateAsync(GeneralState.RECEIVED_BY_DRIVER),
                GeneralState.AWAITING_TRANSFER_TO_CUSTOMER => await _state.GetByStateAsync(GeneralState.DELIVERED),
                _ => throw new ArgumentException("Не правильный статус заказа")
            };
            await _stateHistory.AddAsync(order.SetSecretCodeEmpty());
            return order.Client.User.Id;
        }

        public async Task<Order> RejectAsync(Guid orderId)
        {
            var orderSpec = new OrderForRejectSpecification(orderId);
            var order = await _context.FirstOrDefaultAsync(orderSpec);
            if (order == null)
            {
                return default;
            }
            await _stateHistory.RemoveAsync(order.Id, order.State.Id); 
            await _rejected.AddAsync(order);
            order.State = await _state.GetByStateAsync(GeneralState.WAITING_ON_REVIEW);
            return await _context.UpdateAsync(order
                                        .SetSecretCodeEmpty()
                                        .SetDelivery());
        }

        public async Task SetDeliveryAsync(Order order, Delivery delivery)
        {
            order.State = await _state.GetByStateAsync(GeneralState.ON_REVIEW);
            await _context.UpdateAsync(order.SetDelivery(delivery));
            await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, order.Delivery.Id));
        }

        public async Task<bool> IsOnReview(BackgroundOrder backgroundOrder)
        {
            return await _context
                .AnyAsync(o =>
                    o.Id == backgroundOrder.OrderId &&
                    o.Delivery.Id == backgroundOrder.DeliveryId &&
                    o.State.StateValue == GeneralState.ON_REVIEW);
        }

        public async Task<Order> UpdateStatePendingAsync(Guid orderId)
        {
            var orderSpec = new OrderWithClientSpecification(orderId);
            var order = await _context.FirstOrDefaultAsync(orderSpec)
                    ?? throw new ArgumentException($"Нет такого заказа с Id: {orderId}");
            order.State = await _state.GetByStateAsync(GeneralState.PENDING_For_HAND_OVER);
            await _stateHistory.AddAsync(order.SetSecretCode());
            return order;
        }

        public async Task CancelAsync(Guid orderId)
        {
            var spec = new OrderWithStateSpecification(orderId, GeneralState.WAITING_ON_REVIEW);
            var order = await _context.FirstOrDefaultAsync(spec)
                ?? throw new ArgumentException($"Активный заказ нельзя отменить");
            order.State = await _state.GetByStateAsync(GeneralState.CANCALED);
            await _context.UpdateAsync(order.SetCancellationDate());
        }

        public async Task<Guid> ProfitAsync(ProfitOrderDto dto)
        {
            var spec = new OrderWithStateSpecification(dto.OrderId, dto.UserId);
            var order = await _context.FirstOrDefaultAsync(spec)
                ?? throw new ArgumentException($"У вас нет такой заказа: Id {dto.OrderId}");
            order.State = await _state.GetByStateAsync(GeneralState.AWAITING_TRANSFER_TO_CUSTOMER);
            await _context.UpdateAsync(order.SetSecretCode());
            return order.Client.User.Id;
        }
    }
}