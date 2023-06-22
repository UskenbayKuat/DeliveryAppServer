using ApplicationCore.Models.Entities.Orders;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderForRejectSpecification : Specification<Order>
    {
        public OrderForRejectSpecification(int orderId)
        {
            Query.Include(o => o.State)
                .Include(o => o.Location)
                .Include(o => o.Delivery.Driver)
                .Include(o => o.Route)
                .Include(o => o.Delivery)
                .Where(o => o.Id == orderId);
        }
    }
}