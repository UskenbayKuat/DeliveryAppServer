using ApplicationCore.Entities.AppEntities.Orders;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DeliveryWithLocationSpecification : Specification<Delivery>
    {
        public DeliveryWithLocationSpecification(string userId)
        {
            Query
                .Include(d => d.Location)   
                .Where(d => d.Driver.UserId == userId);
        }
    }
}