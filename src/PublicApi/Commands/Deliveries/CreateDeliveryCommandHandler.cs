using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Models.Dtos.Deliveries;
using AutoMapper;
using MediatR;
using Notification.Interfaces;

namespace PublicApi.Commands.Deliveries
{
    public class CreateDeliveryCommandHandler : AsyncRequestHandler<CreateDeliveryCommand>
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly INotify _notify;

        public CreateDeliveryCommandHandler(
            IMapper mapper, 
            IDeliveryCommand deliveryCommand, 
            INotify notify)
        {
            _mapper = mapper;
            _deliveryCommand = deliveryCommand;
            _notify = notify;
        }

        protected override async Task Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var delivery = await _deliveryCommand.CreateAsync(_mapper.Map<CreateDeliveryDto>(request));
            var orders =await _deliveryCommand.AddWaitingOrdersAsync(delivery);
            if (orders.Any())
            {
                await _notify.SendToDriverAsync(request.UserId, cancellationToken);
            }
        }
    }
}