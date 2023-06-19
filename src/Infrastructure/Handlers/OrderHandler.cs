using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Handlers
{
    public class OrderHandler : IOrderHandler
    {
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly IBackgroundTaskQueue _backgroundTask;
        private readonly IOrderCommand _orderCommand;
        private readonly IContext _context;
        private readonly IOrderContextBuilder _orderContextBuilder;

        public OrderHandler(IDeliveryCommand deliveryCommand, IBackgroundTaskQueue backgroundTask, IOrderCommand orderCommand, IContext context, IOrderContextBuilder orderContextBuilder)
        {
            _deliveryCommand = deliveryCommand;
            _backgroundTask = backgroundTask;
            _orderCommand = orderCommand;
            _context = context;
            _orderContextBuilder = orderContextBuilder;
        }

        public async Task<List<Order>> AddWaitingOrdersToDeliveryAsync(Delivery delivery, CancellationToken cancellationToken)
        {
            var stateOnReview = await _context.FindAsync<State>((int)GeneralState.OnReview);
            var orders = await _orderContextBuilder.StateBuilder()
                .Build()
                .Where(o =>
                    o.Route.Id == delivery.Route.Id &&
                    o.DeliveryDate.Day <= delivery.DeliveryDate.Day &&
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
            var order = await _orderCommand.RejectAsync(orderId);
            return await FindIsNewDeliveryHandlerAsync(order, cancellationToken);
        }
    }
}