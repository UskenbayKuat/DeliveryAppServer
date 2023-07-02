using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IOrderQuery
    {
        public Task<List<DeliveryDto>> GetActiveOrdersForClientAsync(string clientUserId);
        public Task<IReadOnlyList<Order>> GetByDriverUserIdAsync(string driverUserId);
        Task<IReadOnlyList<Order>> GetWaitingOrders(int routeId, DateTime dateTime);
    }
}