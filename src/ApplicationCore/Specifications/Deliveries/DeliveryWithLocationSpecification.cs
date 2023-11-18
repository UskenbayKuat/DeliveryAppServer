using ApplicationCore.Entities.AppEntities.Orders;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DeliveryWithLocationSpecification : Specification<Delivery>
    {
        public DeliveryWithLocationSpecification(Guid userId)
        {
            Query
                .Include(d => d.Location)   
                .Where(d => d.Driver.User.Id == userId);
        }
    }
}