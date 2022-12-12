using System;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.Values
{
    public class RouteInfo
    {   
        public City StartCity { get; set; }
        public City FinishCity { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}