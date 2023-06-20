using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Models.Entities.Orders;

namespace ApplicationCore
{
    public interface IOrderHandler
    {

        public Task<Order> RejectedHandlerAsync(int orderId, CancellationToken cancellationToken);
    }
}