using ApplicationCore.Extensions;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Entities.AppEntities.Cars
{
    public class CarType : BaseEntity
    {
        public CarType(CarTypeEnum carTypeValue)
        {
            Name = carTypeValue.GetDisplayName();
            CarTypeValue = carTypeValue;
            CarTypeName = carTypeValue.ToString();
        }

        public string Name { get; private set; }
        public CarTypeEnum CarTypeValue { get; private set; }
        public string CarTypeName { get; private set; }

    }
}