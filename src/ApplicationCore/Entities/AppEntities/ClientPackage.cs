using System;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.AppEntities
{
    public class ClientPackage : BaseEntity
    {
        public CarType CarType { get; set; }
        public Client Client { get; set; }
        public Package Package { get; set; }

        public bool IsSingle { get; set; }
        public decimal Price { get; set; }
        public Location Location { get; set; }
        public RouteDate RouteDate { get; set; }
        
    }
}