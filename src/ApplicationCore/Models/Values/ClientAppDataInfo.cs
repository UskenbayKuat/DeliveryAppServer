using System.Collections.Generic;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Models.Values
{
    public class ClientAppDataInfo
    {
        public IReadOnlyList<City> Cities { get; set; }
        public IReadOnlyList<CarType> CarTypes { get; set; }
    }
}