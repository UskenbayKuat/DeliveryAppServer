using System;

namespace ApplicationCore.Entities.AppEntities.Routes
{
    public class RouteDate : BaseEntity
    {
        public RouteDate(DateTime createDateTime)
        {
            CreateDateTime = createDateTime;
        }

        public Route Route { get; private set;}
        public DateTime CreateDateTime { get; private set;}

        public RouteDate AddRoute(Route route)
        {
            Route = route;
            return this;
        }
    }
}