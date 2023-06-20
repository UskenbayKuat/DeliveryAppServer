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
        private readonly IOrderCommand _orderCommand;

        public OrderHandler(IDeliveryCommand deliveryCommand, IBackgroundTaskQueue backgroundTask, IOrderCommand orderCommand, IContext context, IOrderContextBuilder orderContextBuilder)
        {
            _orderCommand = orderCommand;
        }
        
        public async Task<Order> RejectedHandlerAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await _orderCommand.RejectAsync(orderId);
            return await FindIsNewDeliveryHandlerAsync(order, cancellationToken);
        }
    }
}