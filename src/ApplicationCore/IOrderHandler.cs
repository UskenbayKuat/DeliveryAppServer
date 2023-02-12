using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;

namespace ApplicationCore
{
    public interface IOrderHandler
    {
        public Task<Order> CreatedHandlerAsync(OrderInfo orderInfo, string userId,
            CancellationToken cancellationToken);

        public Task<Order> RejectedHandlerAsync(int orderId, CancellationToken cancellationToken);
        public Task<List<Order>> SetWaitingOrdersToDeliveryAsync(Delivery delivery, CancellationToken cancellationToken);
    }
}