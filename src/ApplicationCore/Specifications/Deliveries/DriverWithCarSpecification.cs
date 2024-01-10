using ApplicationCore.Entities.AppEntities;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DriverWithCarSpecification : Specification<Driver>
    {
        public DriverWithCarSpecification(Guid userId)
        {
            Query.Include(d => d.Cars).Where(d => d.User.Id == userId);
        }
    }
}