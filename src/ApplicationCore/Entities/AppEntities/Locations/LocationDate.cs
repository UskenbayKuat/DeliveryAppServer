using System;

namespace ApplicationCore.Entities.AppEntities.Locations
{
    public class LocationDate : BaseEntity
    {
        public LocationDate(DateTime locationDateTime)
        {
            LocationDateTime = locationDateTime;
        }

        public Location Location { get; set;}
        public RouteTrip RouteTrip { get; set;}
        public DateTime LocationDateTime { get; private set;}
        
    }
}