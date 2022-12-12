using System;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class RouteTrip : BaseEntity
    {
        public RouteTrip()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        public Driver Driver { get; set;}
        public Route Route { get; set;}
        public DateTime CreatedAt { get; private set; } //TODO add DeliveryDate
        public bool IsActive { get; private set; }

        public RouteTrip ChangeStatusToNotActive()
        {
            IsActive = false;
            return this;
        }
    }
}