using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.Enums
{
    public enum RoleEnum
    {
        [Display(Name = "Админ")]
        Admin,
        [Display(Name = "Водитель")]
        Driver,
        [Display(Name = "Клиент")]
        Client
    }
}
