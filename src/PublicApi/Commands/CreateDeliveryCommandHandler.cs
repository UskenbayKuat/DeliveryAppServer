using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using AutoMapper;
using MediatR;

namespace PublicApi.Commands
{
    public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, string>
    {
        private readonly IMapper _mapper;
        private readonly IRouteTrip _routeTrip;
        private readonly IOrder _order;
        private readonly IContext _context;

        public CreateDeliveryCommandHandler(IMapper mapper, IRouteTrip routeTrip, IOrder order, IContext context)
        {
            _mapper = mapper;
            _routeTrip = routeTrip;
            _order = order;
            _context = context;
        }

        public async Task<string> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var delivery = await _routeTrip.CreateAsync(_mapper.Map<RouteTripInfo>(request), request.UserId);
            if (!await _order.AnyWaitingOrdersAsync(delivery))
            {
                return string.Empty;
            }
            var driverChatHub = await _context.FindAsync<ChatHub>(c => c.UserId == request.UserId);
            return driverChatHub?.ConnectionId;
        }
    }
}