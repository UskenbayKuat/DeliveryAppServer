using System;

namespace PublicApi.Endpoints.Drivers.CreateRouteTrip
{
    public class CreateRouteTripCommand
    {
        public int StartCityId { get; set; }
        public int FinishCityId { get; set; }
        public DateTime TripTime { get; set; }
    }
}