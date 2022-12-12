using System;
using ApplicationCore.Entities.AppEntities.Routes;

namespace PublicApi.Endpoints.Drivers.CreateRouteTrip
{
    public class CreateRouteTripCommand
    {
        public City StartCity { get; set; }
        public City FinishCity { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}