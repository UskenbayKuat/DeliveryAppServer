using System;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Models.Entities.Locations;

namespace ApplicationCore.Entities.Values
{
    public class RouteTripInfo
    {   
        public City StartCity { get; set; }
        public City FinishCity { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Location Location { get; set; }
    }
}