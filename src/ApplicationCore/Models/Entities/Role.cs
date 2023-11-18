using ApplicationCore.Entities;
using ApplicationCore.Extensions;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Models.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; private set; }
        public RoleEnum RoleValue { get; private set; }
        public string RoleName { get; private set; }
        public Role(RoleEnum roleEnum) 
        {
            Name = roleEnum.GetDisplayName();
            RoleValue = roleEnum;
            RoleName = roleEnum.ToString();
        }
        public Role()
        {
            
        }
    }
}
