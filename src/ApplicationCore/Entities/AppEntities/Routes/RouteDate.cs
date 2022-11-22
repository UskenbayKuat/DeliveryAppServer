using System;

namespace ApplicationCore.Entities.AppEntities.Routes
{
    public class RouteDate : BaseEntity
    {
        public RouteDate(DateTime deliveryDate)
        {
            DeliveryDate = deliveryDate;
        }

        public Route Route { get; set;}
        public DateTime DeliveryDate { get; private set;}
        
    }
}