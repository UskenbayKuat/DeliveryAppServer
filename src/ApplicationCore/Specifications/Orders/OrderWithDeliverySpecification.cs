using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Orders
{
    public sealed class OrderWithDeliverySpecification : Specification<Order>
    {
        public OrderWithDeliverySpecification(string driverUserId)
        {
            Query
                .Include(o => o.State)
                .Include(o => o.Route.StartCity)
                .Include(o => o.Client)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.CarType)
                .Include(o => o.Location)
                .Include(o => o.Delivery)
                .Include(o => o.Delivery.State)
                .Include(o => o.Delivery.Route.StartCity)
                .Include(o => o.Delivery.Route.FinishCity)
                .Where(o => o.Delivery.Driver.UserId == driverUserId)
                .Where(o => o.State.StateValue == GeneralState.INPROGRESS ||
                            o.State.StateValue == GeneralState.PENDING_For_HAND_OVER ||
                            o.State.StateValue == GeneralState.RECEIVED_BY_DRIVER||
                            o.State.StateValue == GeneralState.ON_REVIEW);
        }
    }
}