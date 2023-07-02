using ApplicationCore.Entities;

namespace ApplicationCore.Models.Entities.Locations
{
    public class Location : BaseEntity
    {
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public void UpdateLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        
    }
}