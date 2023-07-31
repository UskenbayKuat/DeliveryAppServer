using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Models.Dtos.Deliveries;
using AutoMapper;
using MediatR;
using Notification.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using ApplicationCore.Interfaces.ClientInterfaces;

namespace PublicApi.Commands.Deliveries.Handlers
{
    public class ClientProfitCommandHandler : AsyncRequestHandler<ClientProfitCommand>
    {
        private readonly IMapper _mapper;
        private readonly IOrderCommand _orderCommand;
        private readonly INotify _notify;

        public ClientProfitCommandHandler(
            IMapper mapper,
            IOrderCommand orderCommand,
            INotify notify)
        {
            _mapper = mapper;
            _orderCommand = orderCommand;
            _notify = notify;
        }

        protected override async Task Handle(ClientProfitCommand request, CancellationToken cancellationToken)
        {
            var clientUserId = await _orderCommand.ProfitAsync(_mapper.Map<ClientProfitDto>(request));
            await _notify.SendProfitClientAsync(clientUserId);
        }
    }
}
