using System;

namespace ApplicationCore.Entities.AppEntities.Routes
{
    public class RouteDate : BaseEntity
    {
        public Route Route { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}