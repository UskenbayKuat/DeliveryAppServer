using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Drivers;
using MediatR;
using Notification.Services;
using PublicApi.Commands.Deliveries.Models;

namespace PublicApi.Commands.Deliveries.Handlers
{
    public class StartDeliveryCommandHandler : AsyncRequestHandler<StartDeliveryCommand>
    {
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly INotify _notify;

        public StartDeliveryCommandHandler(IDeliveryCommand deliveryCommand, INotify notify)
        {
            _deliveryCommand = deliveryCommand;
            _notify = notify;
        }

        protected override async Task Handle(StartDeliveryCommand request, CancellationToken cancellationToken)
        {
            await _deliveryCommand.StartAsync(request.UserId);
            await _notify.SendInfoToClientsAsync(request.UserId);
        }
    }
}