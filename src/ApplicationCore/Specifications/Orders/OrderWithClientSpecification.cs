using ApplicationCore.Models.Entities.Orders;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderWithClientSpecification : Specification<Order>
    {
        public OrderWithClientSpecification(int orderId)
        {
            Query.Include(o => o.Client)
                .Where(o => o.Id == orderId);
        }
        public OrderWithClientSpecification(string userId)
        {
            Query.Include(o => o.Client)
                .Where(o => o.Delivery.Driver.UserId == userId);
        }
    }
}