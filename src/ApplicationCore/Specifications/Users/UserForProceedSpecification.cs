using ApplicationCore.Entities.AppEntities;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Users
{
    public sealed class UserForProceedSpecification : Specification<User>
    {
        public UserForProceedSpecification(Guid userId)
        {
            Query
                .Include(x => x.Driver)
                .Include(x => x.Client)
                .Where(x => !x.IsDeleted && x.Id == userId);
        }
    }
}
