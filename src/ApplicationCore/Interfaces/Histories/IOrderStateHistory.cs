using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Histories;
using ApplicationCore.Models.Entities.Orders;

namespace ApplicationCore.Interfaces.Histories
{
    public interface IOrderStateHistory
    {
        Task AddAsync(Order order, State state);
        Task<StateHistoryDto> GetAsync(int orderId);
        Task RemoveAsync(int orderId, int stateId);
    }
}