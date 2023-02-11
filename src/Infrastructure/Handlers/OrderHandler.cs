using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;

namespace Infrastructure.Handlers
{
    public class OrderHandler : IOrderHandler
    {
        
        private readonly IDriver _driverService;
        private readonly IDelivery _delivery;
        private readonly IBackgroundTaskQueue _backgroundTask;
        private readonly IOrder _order;

        public OrderHandler(IDriver driverService, IDelivery delivery, IBackgroundTaskQueue backgroundTask, IOrder order)
        {
            _driverService = driverService;
            _delivery = delivery;
            _backgroundTask = backgroundTask;
            _order = order;
        }

        public async Task<string> RejectedHandlerAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await _driverService.RejectOrderAsync(orderId);
            return await FindDeliveryUserIdAsync(order, cancellationToken);
        }
        public async Task<string> CreatedHandlerAsync(OrderInfo orderInfo,string userId, CancellationToken cancellationToken)
        {
            var order = await _order.CreateAsync(orderInfo, userId, cancellationToken);
            return await FindDeliveryUserIdAsync(order, cancellationToken);
        }

        private async Task<string> FindDeliveryUserIdAsync(Order order, CancellationToken cancellationToken)
        {
            var delivery = await _delivery.FindIsActiveDelivery(order, cancellationToken);
            if (delivery is null)
            {
                return string.Empty;
            }
            await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, delivery.Id));
            await _order.UpdateOrderAsync(order, delivery, (int)GeneralState.OnReview);
            return delivery.RouteTrip.Driver.UserId;
        }
    }
}