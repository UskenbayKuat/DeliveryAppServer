using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities;

namespace ApplicationCore.Models.Entities
{
    public class UserRole : BaseEntity
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
