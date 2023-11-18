using System;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Histories;
using ApplicationCore.Models.Entities.Orders;

namespace ApplicationCore.Interfaces.Clients
{
    public interface IOrderStateHistory
    {
        Task AddAsync(Order order);
        Task<StateHistoryDto> GetAsync(Guid orderId);
        Task RemoveAsync(Guid orderId, Guid stateId);
    }
}