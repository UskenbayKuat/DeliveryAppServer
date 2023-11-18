using ApplicationCore.Extensions;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Entities.AppEntities.Cars
{
    public class CarColor : BaseEntity
    {
        public CarColor(ColorEnum colorValue)
        {
            Name = colorValue.GetDisplayName();
            ColorValue = colorValue;
            ColorName = colorValue.ToString();
        }

        public string Name { get; private set;}
        public ColorEnum ColorValue { get; private set;}
        public string ColorName { get; private set;}
    }
}