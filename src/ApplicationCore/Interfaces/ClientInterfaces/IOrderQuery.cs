using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Models.Entities.Orders;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IOrderQuery
    {
        public Task<ActionResult> GetWaitingOrdersAsync(string clientUserId, CancellationToken cancellationToken);
        public Task<ActionResult> GetActiveOrdersForClientAsync(string userClientId);
        public Task<List<Order>> GetOrdersAsync(string driverUserId);
        Task<IReadOnlyList<Order>> GetWaitingOrders(int routeId, DateTime dateTime);
    }
}