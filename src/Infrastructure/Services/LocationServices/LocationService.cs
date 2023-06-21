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
        private readonly IDeliveryContextBuilder _deliveryContextBuilder;
        private readonly IContext _context;

        public LocationService(IDeliveryContextBuilder deliveryContextBuilder, IContext context)
        {
            _deliveryContextBuilder = deliveryContextBuilder;
            _context = context;
        }

        public async Task<LocationInfo> UpdateDriverLocationAsync(LocationInfo request)
        {
            var delivery = await _deliveryContextBuilder.IncludeLocation()
                .Build()
                .FirstOrDefaultAsync(d => d.Driver.UserId == request.UserId);
            if (request.Latitude != 0 && request.Longitude != 0)
            {
                delivery?.Location.UpdateLocation(request.Latitude, request.Longitude);
                await _context.UpdateAsync(delivery?.Location);
                return request;
            }
            request.Latitude = delivery.Location.Latitude;
            request.Longitude = delivery.Location.Longitude;
            return request;
        }
        /// TODO from LocationDate how Delivery state InProgress
        //private async Task<LocationInfo> UpdateDriverLocationDateAsync(LocationInfo request)
        //{
        //    var locationDate = await _locationDataContextBuilder.LocationBuilder()
        //        .Build()
        //        .FirstOrDefaultAsync(l => l.Delivery.Driver.UserId == request.UserId);
        //    if (request.Latitude != 0 && request.Longitude != 0)
        //    {
        //        locationDate?.Location.UpdateLocation(request.Latitude, request.Longitude);
        //        await _context.UpdateAsync(locationDate);
        //        return request;
        //    }
        //    request.Latitude = locationDate.Location.Latitude;
        //    request.Longitude = locationDate.Location.Longitude;
        //    return request;
        //}
    }
}