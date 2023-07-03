using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.Histories;
using ApplicationCore.Models.Dtos.Histories;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Specifications.Histories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.History
{
    public class OrderStateHistoryService : IOrderStateHistory
    {
        private readonly IAsyncRepository<OrderStateHistory> _context;

        public OrderStateHistoryService(IAsyncRepository<OrderStateHistory> context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order, State state)
        {
            var orderHistory = new OrderStateHistory { Order = order, State = state };
            await _context.AddAsync(orderHistory);
        }

        public async Task<List<StateHistoryDto>> GetAsync(int orderId, int deliveryId)
        {
            var historySpec = new OrderStateHistoryWithStateSpecification(orderId, deliveryId);
            var ordersHistories = await _context
                    .GetQueryableAsync(historySpec)
                    .OrderBy(o => o.CreatedDate)
                    .Select(o => o.GetHistoryDto())
                    .ToListAsync();
            return ordersHistories;
        }
    }
}