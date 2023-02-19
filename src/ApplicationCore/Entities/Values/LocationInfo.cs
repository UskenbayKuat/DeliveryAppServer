namespace ApplicationCore.Entities.Values
{
    public class LocationInfo
    {
        public double Latitude { get;  set; }
        public double Longitude { get;  set; }
        public string UserId { get; private set; }
        public string DriverName { get;  set; }
        public string DriverSurname { get;  set; }
        public string DriverPhoneNumber { get;  set; }
        public LocationInfo SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}