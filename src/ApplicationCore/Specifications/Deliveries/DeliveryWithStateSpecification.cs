using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Enums;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DeliveryWithStateSpecification : Specification<Delivery>
    {
        public DeliveryWithStateSpecification(Guid userId)
        {
            Query.Include(d => d.Orders)
                 .Where(d =>
                     d.Driver.User.Id == userId && (
                     d.State.StateValue == GeneralState.WAITING_ORDER ||
                     d.State.StateValue == GeneralState.INPROGRESS));
        }
        public DeliveryWithStateSpecification(Guid userId, GeneralState state)
        {
            Query.Include(d => d.Orders)
                 .Where(d =>
                     d.Driver.User.Id == userId && 
                     d.State.StateValue == state);
        }
    }
}