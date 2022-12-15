using System;
using ApplicationCore.Entities.AppEntities.Routes;

namespace PublicApi.Endpoints.Drivers.RouteTrip
{
    public class RouteTripCommand
    {
        public City StartCity { get; set; }
        public City FinishCity { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}