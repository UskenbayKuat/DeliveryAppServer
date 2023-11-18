using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderWithDeliverySpecification : Specification<Order>
    {
        public OrderWithDeliverySpecification(Guid clientUserId, bool isUserId)
        {
            Query
                .Include(o => o.Client.User)
                .Include(o => o.State)
                .Include(o => o.Route.StartCity)
                .Include(o => o.Client)
                .Include(o => o.Client.User)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.CarType)
                .Include(o => o.Location)
                .Include(o => o.Delivery).ThenInclude(o => o.Driver).ThenInclude(o => o.User)
                .Include(o => o.Delivery.State)
                .Include(o => o.Delivery.Route.StartCity)
                .Include(o => o.Delivery.Route.FinishCity)
                .Where(o => o.Client.User.Id == clientUserId)
                .Where(o => o.State.StateValue == GeneralState.DELIVERED ||
                            o.State.StateValue == GeneralState.CANCALED);
        }
        public OrderWithDeliverySpecification(Guid orderId)
        {
            Query
                .Include(o => o.Delivery).ThenInclude(o => o.Location)
                .Include(o => o.Delivery).ThenInclude(o => o.Driver).ThenInclude(o => o.User)
                .Where(o => o.Id == orderId);
        }
    }
}