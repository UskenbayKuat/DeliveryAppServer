using System;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.AppEntities
{
    public class RouteTrip : BaseEntity
    {
        public Driver Driver { get; private set;}
        public RouteDate RouteDate { get; private set; }

        public RouteTrip AddRouteTripData(Driver driver, RouteDate routeDate)
        {
            Driver = driver;
            RouteDate = routeDate;
            return this;
        }
        public void UpdateRouteDate(RouteDate routeDate)
        {
            RouteDate = routeDate;
        }
    }
}