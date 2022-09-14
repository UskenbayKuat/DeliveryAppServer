using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities
{
    public class RouteTrip : BaseEntity
    {
        public int StartCityId { get; set; }
        public City StartCity { get; set; }
        public int FinishCityId { get; set; }
        public City FinishCity { get; set; }
        public DateTime TripTime { get; set; }
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public string HubId { get; set; }
    }
}