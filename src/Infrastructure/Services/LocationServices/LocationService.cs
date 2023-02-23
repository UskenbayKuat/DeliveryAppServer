using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.LocationInterfaces;

namespace Infrastructure.Services.LocationServices
{
    public class LocationService : ILocation
    {
        private readonly IContext _context;

        public LocationService(IContext context)
        {
            _context = context;
        }

        public async Task<LocationInfo> SendDriverLocationAsync(LocationInfo request, int deliveryId)
        {
            if (request.Latitude != 0 && request.Longitude != 0)
            {
                return request;
            }
            var locationDate = await _context.FindAsync<LocationData>(l => l.Delivery.Id == deliveryId);
            request.Latitude = locationDate.Location.Latitude;
            request.Longitude = locationDate.Location.Longitude;
            return request;
        }
    }
}