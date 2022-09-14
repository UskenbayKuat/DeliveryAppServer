using System;

namespace ApplicationCore.Entities.ApiEntities
{
    public class RouteInfo
    {   
        public int StartCityId { get; set; }
        public int FinishCityId { get; set; }
        public DateTime TripTime { get; set; }
        public int DriverId { get; set; }
    }
}