using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Orders;

namespace ApplicationCore.Interfaces.Clients
{
    public interface IOrderQuery
    {
        public Task<List<DeliveryDto>> GetActiveOrdersForClientAsync(Guid clientUserId);
        Task<List<DeliveryDto>> GetHistoryAsync(Guid clientUserId);
        Task<IReadOnlyList<Order>> GetWaitingOrders(Guid routeId, DateTime dateTime);
        public Task<IReadOnlyList<Order>> GetByDriverUserIdAsync(Guid driverUserId);
        Task<LocationDto> GetCurrentLocationAsync(Guid orderId);
    }
}