using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderStateHistoryWithStateSpecification : Specification<OrderStateHistory>
    {
        public OrderStateHistoryWithStateSpecification(Guid orderId)
        {
            Query
                .Include(o => o.State)
                .Where(o => o.Order.Id == orderId)
                .Where(o => o.State.StateValue == GeneralState.PENDING_For_HAND_OVER ||
                                         o.State.StateValue == GeneralState.RECEIVED_BY_DRIVER ||
                                         o.State.StateValue == GeneralState.DELIVERED);
        }
    }
}