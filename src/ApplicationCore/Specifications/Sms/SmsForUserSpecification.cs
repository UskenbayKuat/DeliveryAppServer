using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Models.Entities;
using Ardalis.Specification;
using System;

namespace ApplicationCore.Specifications.Sms
{
    public sealed class SmsForUserSpecification : Specification<SmsLog>
    {
        public SmsForUserSpecification(Guid userId)
        {
            Query
                .Include(x => x.User)
                .Where(x => !x.IsDeleted && x.IsActual)
                .Where(x => !x.User.IsDeleted && x.User.Id == userId);
        }
        public SmsForUserSpecification(string phoneNumber, bool isDriver)
        {
            Query
                .Include(x => x.User)
                .Where(x => !x.IsDeleted && x.IsActual)
                .Where(x => !x.User.IsDeleted && 
                             x.User.PhoneNumber == phoneNumber &&
                             x.User.IsDriver == isDriver);
        }
    }
}
