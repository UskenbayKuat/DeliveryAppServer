using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notification.Interfaces;

namespace PublicApi.Commands
{
    public class StartDeliveryCommandHandler : IRequestHandler<StartDeliveryCommand, ActionResult>
    {
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly INotify _notify;

        public StartDeliveryCommandHandler(IDeliveryCommand deliveryCommand, INotify notify)
        {
            _deliveryCommand = deliveryCommand;
            _notify = notify;
        }

        public async Task<ActionResult> Handle(StartDeliveryCommand request, CancellationToken cancellationToken)
        {
            await _deliveryCommand.StartAsync(request.UserId);
            await _notify.SendInfoToClientsAsync(request.UserId);
            return new NoContentResult();
        }
    }
}