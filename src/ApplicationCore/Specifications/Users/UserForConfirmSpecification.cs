using ApplicationCore.Entities.AppEntities;
using Ardalis.Specification;
namespace ApplicationCore.Specifications.Users
{
    public sealed class UserForConfirmSpecification : Specification<User>
    {
        public UserForConfirmSpecification(string phoneNumber, bool isDriver)
        {
            Query
                .Include(x => x.Driver)
                .Include(x => x.Client)
                .Where(x => !x.IsDeleted && x.PhoneNumber == phoneNumber)
                .Where(x => x.IsDriver == isDriver);
            if (isDriver)
            {
                Query
                    .Include(x => x.Driver)
                    .Where(x => !x.Driver.IsDeleted);
            }
            else
            {
                Query
                    .Include(x => x.Client)
                    .Where(x => !x.Client.IsDeleted);
            }
        }
    }
}
