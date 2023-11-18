using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderForRejectSpecification : Specification<Order>
    {
        public OrderForRejectSpecification(Guid orderId)
        {
            Query.Include(o => o.State)
                .Include(o => o.Location)
                .Include(o => o.Delivery.Driver)
                .Include(o => o.Route)
                .Include(o => o.Delivery)
                .Where(o => o.Id == orderId)
                .Where(o => o.State.StateValue == GeneralState.ON_REVIEW ||
                                 o.State.StateValue == GeneralState.PENDING_For_HAND_OVER);
        }
    }
}