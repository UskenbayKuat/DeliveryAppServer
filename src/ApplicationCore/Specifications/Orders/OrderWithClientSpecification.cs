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
                .Include(o => o.Delivery).ThenInclude(x => x.Driver)
                .Include(o => o.Delivery.State)
                .Where(o => o.Delivery.Driver.UserId == userId && !o.Delivery.IsDeleted)
                .Where(o => o.Delivery.State.StateValue != Models.Enums.GeneralState.CANCALED)
                .Where(o => o.Delivery.State.StateValue != Models.Enums.GeneralState.DONE);
        }
    }
}