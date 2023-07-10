using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Histories
{
    public sealed class OrderStateHistoryWithStateSpecification : Specification<OrderStateHistory>
    {
        public OrderStateHistoryWithStateSpecification(int orderId)
        {
            Query
                .Include(o => o.State)
                .Where(o => o.Order.Id == orderId)
                .Where(o => o.State.StateValue == GeneralState.PendingForHandOver ||
                                         o.State.StateValue == GeneralState.ReceivedByDriver ||
                                         o.State.StateValue == GeneralState.Done);
        }
    }
}