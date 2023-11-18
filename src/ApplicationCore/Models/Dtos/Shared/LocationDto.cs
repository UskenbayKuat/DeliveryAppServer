using System;

namespace ApplicationCore.Models.Dtos.Shared
{
    public class LocationDto
    {
        public double Latitude { get;  set; }
        public double Longitude { get;  set; }
        public Guid UserId { get; private set; }
        public string DriverName { get;  set; }
        public string DriverSurname { get;  set; }
        public string DriverPhoneNumber { get;  set; }
        public Guid OrderId { get; set; }
        public LocationDto SetUserId(string userId)
        {
            UserId = Guid.Parse(userId);
            return this;
        }
    }
}