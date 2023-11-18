using ApplicationCore.Models.Entities.Orders;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderWithClientSpecification : Specification<Order>
    {
        public OrderWithClientSpecification(Guid orderId)
        {
            Query.Include(o => o.Client).ThenInclude(o => o.User)
                .Where(o => o.Id == orderId);
        }
        public OrderWithClientSpecification(Guid userId, bool isUserId)
        {
            Query.Include(o => o.Client).ThenInclude(o => o.User)
                .Include(o => o.Delivery).ThenInclude(x => x.Driver)
                .Include(o => o.Delivery.State)
                .Where(o => o.Delivery.Driver.User.Id == userId && !o.Delivery.IsDeleted)
                .Where(o => o.Delivery.State.StateValue != Models.Enums.GeneralState.CANCALED)
                .Where(o => o.Delivery.State.StateValue != Models.Enums.GeneralState.DONE);
        }
    }
}