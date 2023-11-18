using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ApplicationCore.Models.Enums
{
    public enum CarTypeEnum
    {
        [Display(Name = "Седан")]
        Sedan,

        [Display(Name = "Минивэн")]
        Minivan,

        [Display(Name = "Фургон")]
        Van
    }
}
