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
        public Task<List<DeliveryDto>> GetActiveOrdersForClientAsync(string clientUserId);
        Task<List<DeliveryDto>> GetHistoryAsync(string clientUserId);
        Task<IReadOnlyList<Order>> GetWaitingOrders(int routeId, DateTime dateTime);
        public Task<IReadOnlyList<Order>> GetByDriverUserIdAsync(string driverUserId);
        Task<LocationDto> GetCurrentLocationAsync(LocationDto dto);
    }
}