using System;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Entities.Locations;

namespace ApplicationCore.Entities.AppEntities.Locations
{
    public class LocationData : BaseEntity
    {
        public LocationData()
        {
            LocationDateTime = DateTime.Now;
        }

        public Location Location { get; set;}
        public Delivery Delivery { get; set;}
        public DateTime LocationDateTime { get; private set;}
        
    }
}