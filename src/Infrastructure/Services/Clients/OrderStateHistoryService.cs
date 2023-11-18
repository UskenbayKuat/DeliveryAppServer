using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Models.Dtos.Histories;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Specifications.Orders;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Clients
{
    public class OrderStateHistoryService : IOrderStateHistory
    {
        private readonly IAsyncRepository<OrderStateHistory> _context;

        public OrderStateHistoryService(IAsyncRepository<OrderStateHistory> context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            var orderHistory = new OrderStateHistory { Order = order, State = order.State };
            await _context.AddAsync(orderHistory);
        }

        public async Task RemoveAsync(int orderId, int stateId)
        {
            var orderHistory = await _context
                .FirstOrDefaultAsync(o => o.Order.Id == orderId && o.State.Id == stateId)
                ?? throw new ArgumentException("Не найдено статус заказа");
            await _context.RemoveAsync(orderHistory);
        }

        public async Task<StateHistoryDto> GetAsync(int orderId)
        {
            var historySpec = new OrderStateHistoryWithStateSpecification(orderId);
            var ordersHistories = await _context
                    .GetQueryableAsync(historySpec)
                    .OrderBy(o => o.CreatedDate)
                    .ToListAsync();
            return ordersHistories.GetStateHistoryDto();
        }
    }
}