using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Interfaces.ContextInterfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PublicApi.Helpers;

namespace PublicApi.Commands
{
    public class LocationCommandHandler : AsyncRequestHandler<LocationCommand>
    {
        private readonly HubHelper _hubHelper;
        private readonly IOrderContextBuilder _orderContextBuilder;
        private readonly IContext _context;

        public LocationCommandHandler(HubHelper hubHelper, IOrderContextBuilder orderContextBuilder, IContext context)
        {
            _hubHelper = hubHelper;
            _orderContextBuilder = orderContextBuilder;
            _context = context;
        }

        protected override async Task Handle(LocationCommand request, CancellationToken cancellationToken)
        {
            var orders = await _orderContextBuilder.ClientAndDeliveryBuilder()
                .Build()
                .Where(o => o.Delivery.Driver.UserId == request.UserId)
                .ToListAsync(cancellationToken); ;
            var locationCommand =  await CurrentLocationAsync(orders.FirstOrDefault()!.Delivery.Id, request, cancellationToken);
            await _hubHelper.SendDriverLocationToClientsAsync(orders, locationCommand);
        }
        
        private async Task<LocationCommand> CurrentLocationAsync(int routeTripId, LocationCommand locationCommand, CancellationToken cancellationToken)
        {
            if (locationCommand.Latitude != 0 && locationCommand.Longitude != 0)
            {
                return locationCommand;
            }
            var locationDate = await _context.FindAsync<LocationData>(l => l.Delivery.Id == routeTripId);
            locationCommand.Latitude = locationDate.Location.Latitude;
            locationCommand.Longitude = locationDate.Location.Longitude;
            return locationCommand;
        }
    }
}