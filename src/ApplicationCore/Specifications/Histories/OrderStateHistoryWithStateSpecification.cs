using ApplicationCore.Models.Entities.Orders;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Histories
{
    public sealed class OrderStateHistoryWithStateSpecification : Specification<OrderStateHistory>
    {
        public OrderStateHistoryWithStateSpecification(int orderId)
        {
            Query
                .Include(o => o.State)
                .Where(o => o.Order.Id == orderId);
        }
    }
}