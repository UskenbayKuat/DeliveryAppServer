using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Values.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DeliveryWithOrderSpecification : Specification<Delivery>
    {
        public DeliveryWithOrderSpecification(string userId)
        {
            Query
                .Include(d => d.Route.StartCity)
                .Include(d => d.Route.FinishCity)
                .Include(d => d.Orders)
                .ThenInclude(o => o.State)
                .Include(d => d.Orders)
                .ThenInclude(o => o.Route.StartCity)
                .Include(d => d.Orders)
                .ThenInclude(o => o.Client)
                .Include(d => d.Orders)
                .ThenInclude(o => o.Route.FinishCity)
                .Include(d => d.Orders)
                .ThenInclude(o => o.Package)
                .Include(d => d.Orders)
                .ThenInclude(o => o.CarType)
                .Include(d => d.Orders)
                .ThenInclude(o => o.Location)
                .Where(d => d.Driver.UserId == userId)
                .Where(o => o.State.StateValue == GeneralState.InProgress ||
                            o.State.StateValue == GeneralState.WaitingOrder)
                ;
        }
    }
}