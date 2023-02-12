using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DriverInterfaces;
using AutoMapper;
using MediatR;
using PublicApi.Helpers;

namespace PublicApi.Commands
{
    public class CreateDeliveryCommandHandler : AsyncRequestHandler<CreateDeliveryCommand>
    {
        private readonly IMapper _mapper;
        private readonly IRouteTrip _routeTrip;
        private readonly IOrderHandler _orderHandler;
        private readonly HubHelper _hubHelper;

        public CreateDeliveryCommandHandler(IMapper mapper, IRouteTrip routeTrip, IOrderHandler orderHandler, HubHelper hubHelper)
        {
            _mapper = mapper;
            _routeTrip = routeTrip;
            _orderHandler = orderHandler;
            _hubHelper = hubHelper;
        }

        protected override async Task Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var delivery = await _routeTrip.CreateAsync(_mapper.Map<RouteTripInfo>(request), request.UserId);
            var orders = await _orderHandler.SetWaitingOrdersToDeliveryAsync(delivery, cancellationToken);
            if (orders.Any())
            {
                await _hubHelper.SendToDriverAsync(request.UserId, cancellationToken);
            }
        }
    }
}