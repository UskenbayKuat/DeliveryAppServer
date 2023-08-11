using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IOrderQuery
    {
        public Task<List<DeliveryDto>> GetActiveOrdersForClientAsync(string clientUserId);
        Task<List<DeliveryDto>> GetHistoryAsync(string clientUserId);
        Task<IReadOnlyList<Order>> GetWaitingOrders(int routeId, DateTime dateTime);
        public Task<IReadOnlyList<Order>> GetByDriverUserIdAsync(string driverUserId);
    }
}