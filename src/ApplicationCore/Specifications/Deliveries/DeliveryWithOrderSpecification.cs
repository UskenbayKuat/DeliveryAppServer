using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;
using System.Linq;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DeliveryWithOrderSpecification : Specification<Delivery>
    {
        public DeliveryWithOrderSpecification(string userId, bool isActive = true)
        {
            Query
                .Include(d => d.State)
                .Include(d => d.Route.StartCity)
                .Include(d => d.Route.FinishCity)
                .Include(d => d.Orders).ThenInclude(o => o.State)
                .Include(d => d.Orders).ThenInclude(o => o.Route.StartCity)
                .Include(d => d.Orders).ThenInclude(o => o.Client)
                .Include(d => d.Orders).ThenInclude(o => o.Route.FinishCity)
                .Include(d => d.Orders).ThenInclude(o => o.Package)
                .Include(d => d.Orders).ThenInclude(o => o.CarType)
                .Include(d => d.Orders).ThenInclude(o => o.Location)
                .Where(d => d.Driver.UserId == userId && !d.IsDeleted);
            if (isActive)
            {
                Query
                    .Where(d => d.State.StateValue == GeneralState.INPROGRESS ||
                                d.State.StateValue == GeneralState.WAITING_ORDER);
                    //.Where(d => d.Orders.Any(o => o.State.StateValue == GeneralState.WAITING_ON_REVIEW ||
                    //                              o.State.StateValue == GeneralState.ON_REVIEW ||
                    //                              o.State.StateValue == GeneralState.PENDING_For_HAND_OVER ||
                    //                              o.State.StateValue == GeneralState.RECEIVED_BY_DRIVER));
            }
            else
            {
                Query
                    .Where(d => d.State.StateValue == GeneralState.DONE ||
                                d.State.StateValue == GeneralState.CANCALED);
            }


        }
    }
}