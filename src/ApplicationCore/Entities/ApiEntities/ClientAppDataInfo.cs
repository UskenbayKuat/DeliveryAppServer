using System.Collections.Generic;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.ApiEntities
{
    public class ClientAppDataInfo
    {
        public List<City> Cities { get; set; }
        public List<CarType> CarTypes { get; set; }
    }
}