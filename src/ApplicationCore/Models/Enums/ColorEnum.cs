using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models.Enums
{
    public enum ColorEnum
    {
        [Display(Name = "Черный")]
        Black,

        [Display(Name = "Белый")]
        White,

        [Display(Name = "Серый")]
        Gray,

        [Display(Name = "Красный")]
        Red,

        [Display(Name = "Бордовый")]
        Burgundy,

        [Display(Name = "Зеленый")]
        Green,

        [Display(Name = "Синий")]
        Blue,

        [Display(Name = "Фиолетовый")]
        Purple
    }
}
