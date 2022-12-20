using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IOrder
    {
        public Task<List<OrderInfo>> FindWaitingOrdersAsync(string driverUserId);
        public Task<ActionResult> CreateAsync(OrderInfo info, string clientUserId, Func<string, Task> func, CancellationToken cancellationToken);
        public Task<ActionResult> GetWaitingOrdersAsync(string clientUserId, CancellationToken cancellationToken);
        public Task<ActionResult> GetOnReviewOrdersAsync(string clientUserId, CancellationToken cancellationToken);
    }
}