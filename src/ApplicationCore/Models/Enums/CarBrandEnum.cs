using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ApplicationCore.Models.Enums
{
    public enum CarBrandEnum
    {
        [Display(Name = "BMW")]
        BMW,

        [Display(Name = "Mercedes")]
        Mercedes,

        [Display(Name = "Audi")]
        Audi,

        [Display(Name = "Toyota")]
        Toyota,

        [Display(Name = "Subaru")]
        Subaru,

        [Display(Name = "Mitsubishi")]
        Mitsubishi,

        [Display(Name = "Ford")]
        Ford,

        [Display(Name = "Daweoo")]
        Daweoo,

        [Display(Name = "Lada")]
        Lada
    }
}
