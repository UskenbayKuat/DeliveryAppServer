using System.Collections.Generic;
using ApplicationCore.Entities.AppEntities;

namespace ApplicationCore.Entities.ApiEntities
{
    public class ClientAppDataInfo
    {
        public List<City> Cities { get; set; }
        public List<CarType> CarTypes { get; set; }
    }
}