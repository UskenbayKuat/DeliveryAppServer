using ApplicationCore.Models.Dtos.Deliveries;
using AutoMapper;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using PublicApi.Commands.Deliveries.Models;
using ApplicationCore.Interfaces.Clients;
using Notification.Services;

namespace PublicApi.Commands.Deliveries.Handlers
{
    public class ClientProfitCommandHandler : AsyncRequestHandler<ProfitOrderCommand>
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

        protected override async Task Handle(ProfitOrderCommand request, CancellationToken cancellationToken)
        {
            var clientUserId = await _orderCommand.ProfitAsync(_mapper.Map<ProfitOrderDto>(request));
            await _notify.SendProfitClientAsync(clientUserId);
        }
    }
}
