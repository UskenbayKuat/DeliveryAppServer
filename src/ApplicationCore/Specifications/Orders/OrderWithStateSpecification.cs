using System;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderWithStateSpecification : Specification<Order>
    {
        public OrderWithStateSpecification(int routeId, DateTime date)
        {
            Query.Include(o => o.State)
                .Where(o =>
                    o.Route.Id == routeId &&
                    o.DeliveryDate <= date &&
                    o.State.StateValue == GeneralState.WaitingOnReview);
        }
        public OrderWithStateSpecification(int orderId)
        {
            Query.Include(o => o.State)
                .Where(o => o.Id == orderId);
        }
    }
}