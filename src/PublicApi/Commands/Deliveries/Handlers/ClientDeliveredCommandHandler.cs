using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Models.Dtos.Deliveries;
using AutoMapper;
using MediatR;
using Notification.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Commands.Deliveries.Handlers
{
    public class ClientDeliveredCommandHandler : AsyncRequestHandler<ClientDeliveredCommand>
    {
        private readonly IMapper _mapper;
        private readonly IOrderCommand _orderCommand;
        private readonly INotify _notify;

        public ClientDeliveredCommandHandler(
            IMapper mapper,
            IOrderCommand orderCommand,
            INotify notify)
        {
            _mapper = mapper;
            _orderCommand = orderCommand;
            _notify = notify;
        }

        protected override async Task Handle(ClientDeliveredCommand request, CancellationToken cancellationToken)
        {
            var clientUserId = await _orderCommand.DeliveredAsync(_mapper.Map<ClientDeliveredDto>(request));
            await _notify.SendProfitClientAsync(clientUserId);
        }
    }
}
