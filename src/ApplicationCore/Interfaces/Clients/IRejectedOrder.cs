using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Entities.Orders;

namespace ApplicationCore.Interfaces.Clients
{
    public interface IRejectedOrder
    {
        Task<bool> CheckRejectedAsync(Guid deliveryId, Guid orderId);
        Task AddAsync(Order order);
    }
}