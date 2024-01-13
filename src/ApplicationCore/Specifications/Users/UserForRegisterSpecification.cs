using ApplicationCore.Entities.AppEntities;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Users
{
    public sealed class UserForRegisterSpecification : Specification<User>
    {
        public UserForRegisterSpecification(string phoneNumber, bool isDriver)
        {
            Query.Where(x => !x.IsDeleted && x.PhoneNumber == phoneNumber)
                 .Where(x => x.IsDriver == isDriver);
        }
    }
}
