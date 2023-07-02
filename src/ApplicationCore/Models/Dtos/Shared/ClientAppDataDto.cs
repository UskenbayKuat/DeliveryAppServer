using System.Collections.Generic;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Models.Dtos.Shared
{
    public class ClientAppDataDto
    {
        public IReadOnlyList<City> Cities { get; set; }
        public IReadOnlyList<CarType> CarTypes { get; set; }
    }
}