using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;

namespace Notification.Interfaces
{
    public interface INotify
    {
        public Task SendToDriverAsync(string userId, CancellationToken cancellationToken);
        public Task SendToClient(string userId, CancellationToken cancellationToken);
        public Task SendDriverLocationToClientsAsync(List<Order> orders, LocationInfo locationCommand);

    }
}