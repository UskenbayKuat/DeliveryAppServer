using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderWithDeliverySpecification : Specification<Order>
    {
        public OrderWithDeliverySpecification(string clientUserId)
        {
            Query
                .Include(o => o.State)
                .Include(o => o.Route.StartCity)
                .Include(o => o.Client)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.CarType)
                .Include(o => o.Location)
                .Include(o => o.Delivery).ThenInclude(o => o.Driver)
                .Include(o => o.Delivery.State)
                .Include(o => o.Delivery.Route.StartCity)
                .Include(o => o.Delivery.Route.FinishCity)
                .Where(o => o.Client.UserId == clientUserId)
                .Where(o => o.State.StateValue == GeneralState.DELIVERED ||
                            o.State.StateValue == GeneralState.CANCALED);
        }
    }
}