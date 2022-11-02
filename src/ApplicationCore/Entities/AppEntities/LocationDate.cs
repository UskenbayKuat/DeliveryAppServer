using System;

namespace ApplicationCore.Entities.AppEntities
{
    public class LocationDate : BaseEntity
    {
        public Location Location { get; set; }
        public RouteTrip RouteTrip { get; set; }
        public DateTime LocationDateTime { get; set; }
    }
}