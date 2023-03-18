using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.LocationInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.LocationServices
{
    public class LocationService : ILocation
    {
        private readonly ILocationDataContextBuilder _locationDataContextBuilder;
        private readonly IContext _context;

        public LocationService(ILocationDataContextBuilder locationDataContextBuilder, IContext context)
        {
            _locationDataContextBuilder = locationDataContextBuilder;
            _context = context;
        }

        public async Task<LocationInfo> UpdateDriverLocationAsync(LocationInfo request)
        {
            var locationDate = await  _locationDataContextBuilder.LocationBuilder()
                .Build()
                .FirstOrDefaultAsync(l => l.Delivery.Driver.UserId == request.UserId);
            if (request.Latitude != 0 && request.Longitude != 0)
            {
                locationDate?.Location.UpdateLocation(request.Latitude, request.Longitude);
                await _context.UpdateAsync(locationDate);
                return request;
            }
            request.Latitude = locationDate.Location.Latitude;
            request.Longitude = locationDate.Location.Longitude;
            return request;
        }
    }
}