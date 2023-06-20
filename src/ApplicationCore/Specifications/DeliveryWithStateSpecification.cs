using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Values.Enums;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
    public sealed class DeliveryWithStateSpecification : Specification<Delivery>
    {
        public DeliveryWithStateSpecification(int driverId)
        {
            Query.Where(d =>
                d.Driver.Id == driverId && (
                    d.State.StateValue == GeneralState.Waiting ||
                    d.State.StateValue == GeneralState.InProgress));
        }
        public DeliveryWithStateSpecification(string userId)
        {
            Query.Include(d => d.Orders)
                 .Where(d =>
                     d.Driver.UserId == userId && (
                     d.State.StateValue == GeneralState.Waiting ||
                     d.State.StateValue == GeneralState.InProgress));
        }
    }
}