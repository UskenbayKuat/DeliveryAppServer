using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDelivery
    {

        public Task<ActionResult> GetActiveDeliveriesForClient(string userClientId, CancellationToken cancellationToken);
        public Task<string> AddToDeliveryAsync(int orderId);

    }
}