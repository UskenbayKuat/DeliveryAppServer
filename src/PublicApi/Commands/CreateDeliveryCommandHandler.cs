using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using AutoMapper;
using MediatR;
using Notification.Interfaces;

namespace PublicApi.Commands
{
    public class CreateDeliveryCommandHandler : AsyncRequestHandler<CreateDeliveryCommand>
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly IOrderHandler _orderHandler;
        private readonly INotify _notify;

        public CreateDeliveryCommandHandler(IMapper mapper, IDeliveryCommand deliveryCommand, IOrderHandler orderHandler, INotify notify)
        {
            _mapper = mapper;
            _deliveryCommand = deliveryCommand;
            _orderHandler = orderHandler;
            _notify = notify;
        }

        protected override async Task Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var delivery = await _deliveryCommand.CreateAsync(_mapper.Map<RouteTripInfo>(request), request.UserId);
            var orders = await _orderHandler.AddWaitingOrdersToDeliveryAsync(delivery, cancellationToken);
            if (orders.Any())
            {
                await _notify.SendToDriverAsync(request.UserId, cancellationToken);
            }
        }
    }
}