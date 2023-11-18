using System;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Entities.Locations;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DeliveryWithDriverSpecification : Specification<Delivery>
    {
        public DeliveryWithDriverSpecification(Guid routeId, DateTime dateTime, Location location)
        {
            Query
                .Include(d => d.Driver)
                .Include(d => d.Driver.User)
                .OrderBy(d => Math.Abs(d.Location.Latitude - location.Latitude) + Math.Abs(d.Location.Longitude - location.Longitude))
                .Where(d =>
                    d.Route.Id == routeId &&
                    d.DeliveryDate >= dateTime &&
                    d.State.StateValue == GeneralState.WAITING_ORDER);
        }
    }
}