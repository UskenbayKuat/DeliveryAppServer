using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Histories;
using ApplicationCore.Models.Entities.Orders;

namespace ApplicationCore.Interfaces.Histories
{
    public interface IOrderStateHistory
    {
        Task AddAsync(Order order, State state);
        Task<List<StateHistoryDto>> GetAsync(int orderId, int deliveryId);
    }
}