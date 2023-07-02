using System.Collections.Generic;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Models.Dtos.Shared
{
    public class DriverAppDataDto
    {
        public IReadOnlyList<City> Cities { get; set; }
        public IReadOnlyList<Kit> Kits { get; set; }
        public IReadOnlyList<CarType> CarTypes { get; set; }
        public IReadOnlyList<CarBrand> CarBrands { get; set; }
        public IReadOnlyList<CarColor> CarColors { get; set; }
    }
}