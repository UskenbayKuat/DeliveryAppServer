using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IOrderHandler
    {
        public Task<string> CreatedHandlerAsync(OrderInfo orderInfo, string userId,
            CancellationToken cancellationToken);

        public Task<string> RejectedHandlerAsync(int orderId, CancellationToken cancellationToken);
    }
}