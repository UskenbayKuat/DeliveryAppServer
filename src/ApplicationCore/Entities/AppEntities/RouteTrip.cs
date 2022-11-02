using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities
{
    public class RouteTrip : BaseEntity
    {
        public string HubId { get; set; }
        public Driver Driver { get; set; }
        public RouteDate RouteDate { get; set; }

    }
}