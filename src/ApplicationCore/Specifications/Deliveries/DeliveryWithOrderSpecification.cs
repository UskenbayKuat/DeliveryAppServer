using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;
using System;
using System.Linq;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DeliveryWithOrderSpecification : Specification<Delivery>
    {
        public DeliveryWithOrderSpecification(Guid userId, bool isActive = true)
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
                .Include(d => d.Driver).ThenInclude(o => o.User)
                .Where(d => d.Driver.User.Id == userId && !d.IsDeleted);
            if (isActive)
            {
                Query
                    .Where(d => d.State.StateValue == GeneralState.INPROGRESS ||
                                d.State.StateValue == GeneralState.WAITING_ORDER);
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