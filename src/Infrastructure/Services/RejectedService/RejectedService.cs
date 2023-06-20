using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.RejectedInterfaces;
using ApplicationCore.Models.Entities.Orders;

namespace Infrastructure.Services.RejectedService
{
    public class RejectedService : IRejected
    {
        private readonly IAsyncRepository<RejectedOrder> _context;

        public RejectedService(IAsyncRepository<RejectedOrder> context)
        {
            _context = context;
        }

        public async Task<bool> CheckRejectedAsync(int deliveryId, int orderId)
        {
            return await _context.AnyAsync(r =>
                    r.Delivery.Id == deliveryId &&
                    r.Order.Id == orderId);
        }

        public async Task AddAsync(Order order)
        {
            await _context.AddAsync(new RejectedOrder
            {
                Order = order,
                Delivery = order.Delivery
            });
        }
    }
}