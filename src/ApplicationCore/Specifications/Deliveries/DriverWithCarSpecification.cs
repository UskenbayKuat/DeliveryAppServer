using ApplicationCore.Entities.AppEntities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications.Deliveries
{
    public sealed class DriverWithCarSpecification : Specification<Driver>
    {
        public DriverWithCarSpecification(string userId)
        {
            Query.Include(d => d.Car).Where(d => d.UserId == userId);
        }
    }
}