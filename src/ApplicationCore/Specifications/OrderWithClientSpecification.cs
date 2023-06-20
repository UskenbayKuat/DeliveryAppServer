using ApplicationCore.Models.Entities.Orders;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public sealed class OrderWithClientSpecification : Specification<Order>
    {
        public OrderWithClientSpecification(int orderId)
        {
            Query.Include(o => o.Client)
                .Where(o => o.Id == orderId);
        }
    }
}