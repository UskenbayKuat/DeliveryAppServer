using ApplicationCore.Extensions;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Entities.AppEntities.Cars
{
    public class CarBrand : BaseEntity
    {
        public CarBrand(CarBrandEnum carBrandValue)
        {
            Name = carBrandValue.GetDisplayName();
            CarBrandValue = carBrandValue;
            CarBrandName = carBrandValue.ToString();
        }

        public string Name { get; private set; }
        public CarBrandEnum CarBrandValue { get; private set; }
        public string CarBrandName { get; private set; }
    }
}