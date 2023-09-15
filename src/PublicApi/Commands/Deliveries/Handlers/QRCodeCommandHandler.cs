using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Models.Dtos.Deliveries;
using AutoMapper;
using MediatR;
using Notification.Services;
using PublicApi.Commands.Deliveries.Models;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Commands.Deliveries.Handlers
{
    public class QRCodeCommandHandler : AsyncRequestHandler<QRCodeCommand>
    {

        private readonly IOrderCommand _orderCommand;
        private readonly IMapper _mapper;
        private readonly INotify _notify;
        public QRCodeCommandHandler(
            IOrderCommand orderCommand, 
            IMapper mapper, 
            INotify notify)
        {
            _orderCommand = orderCommand;
            _mapper = mapper;
            _notify = notify;
        }

        protected override async Task Handle(QRCodeCommand request, CancellationToken cancellationToken)
        {
            var clientId = await _orderCommand.QRCodeAcceptAsync(_mapper.Map<QRCodeDto>(request));
            await _notify.QrCodeClientAsync(clientId);
        }
    }
}
