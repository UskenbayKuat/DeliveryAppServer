using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDelivery
    {

        public Task<ActionResult> GetActiveOrdersForClientAsync(string userClientId);
        public Task<ActionResult> AddToDeliveryAsync(int orderId, Func<string, Task> func);

    }
}