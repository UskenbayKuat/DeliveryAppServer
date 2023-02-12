using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Handlers
{
    public class OrderHandler : IOrderHandler
    {
        
        private readonly IDriver _driverService;
        private readonly IDelivery _delivery;
        private readonly IBackgroundTaskQueue _backgroundTask;
        private readonly IOrder _order;
        private readonly IContext _context;

        public OrderHandler(IDriver driverService, IDelivery delivery, IBackgroundTaskQueue backgroundTask, IOrder order, IContext context)
        {
            _driverService = driverService;
            _delivery = delivery;
            _backgroundTask = backgroundTask;
            _order = order;
            _context = context;
        }



        public async Task<List<Order>> SetWaitingOrdersToDeliveryAsync(Delivery delivery, CancellationToken cancellationToken)
        {
            var stateOnReview = await _context.FindAsync<State>((int)GeneralState.OnReview);
            var orders = await _context.Orders()
                .IncludeOrdersInfoBuilder()
                .Where(o =>
                    o.Route.Id == delivery.RouteTrip.Route.Id &&
                    o.DeliveryDate.Day <= delivery.RouteTrip.DeliveryDate.Day &&
                    o.State.Id == (int)GeneralState.Waiting).ToListAsync(cancellationToken);
            foreach (var order in orders)
            {
                order.State = stateOnReview;
                delivery.AddOrder(order);
                await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, delivery.Id));
            }
            await _context.UpdateAsync(delivery);
            return orders;
        }
        
        public async Task<Order> RejectedHandlerAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await _driverService.RejectOrderAsync(orderId);
            return await SetDeliveryAsync(order, cancellationToken);
        }
        public async Task<Order> CreatedHandlerAsync(OrderInfo orderInfo,string userId, CancellationToken cancellationToken)
        {
            var order = await _order.CreateAsync(orderInfo, userId, cancellationToken);
            return await SetDeliveryAsync(order, cancellationToken);
        }

        private async Task<Order> SetDeliveryAsync(Order order, CancellationToken cancellationToken)
        {
            order.Delivery = await _delivery.FindIsActiveDelivery(order, cancellationToken);
            if (order.Delivery != null)
            {
                await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, order.Delivery.Id));
                order.State = await _context.FindAsync<State>((int)GeneralState.OnReview);
                await _context.UpdateAsync(order);
            }
            return order;
        }
    }
}