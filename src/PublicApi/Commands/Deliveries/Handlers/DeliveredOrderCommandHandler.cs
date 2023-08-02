using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Models.Dtos.Deliveries;
using AutoMapper;
using MediatR;
using Notification.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Commands.Deliveries.Handlers
{
    public class DeliveredOrderCommandHandler : AsyncRequestHandler<DeliveredOrderCommand>
    {
        private readonly IMapper _mapper;
        private readonly IOrderCommand _orderCommand;
        private readonly INotify _notify;

        public DeliveredOrderCommandHandler(
            IMapper mapper,
            IOrderCommand orderCommand,
            INotify notify)
        {
            _mapper = mapper;
            _orderCommand = orderCommand;
            _notify = notify;
        }

        protected override async Task Handle(DeliveredOrderCommand request, CancellationToken cancellationToken)
        {
            var clientUserId = await _orderCommand.DeliveredAsync(_mapper.Map<DeliveredOrderDto>(request));
            await _notify.SendProfitClientAsync(clientUserId);
        }
    }
}
