using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using MediatR;
using Notification.Interfaces;

namespace PublicApi.Commands
{
    public class ConfirmOrderCommandHandler : AsyncRequestHandler<ConfirmOrderCommand>
    {
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly INotify _notify;


        public ConfirmOrderCommandHandler(IDeliveryCommand deliveryCommand, INotify notify)
        {
            _deliveryCommand = deliveryCommand;
            _notify = notify;
        }

        protected override async Task Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order  = await _deliveryCommand.AddOrderAsync(request.OrderId);
            await _notify.SendToClient(order.Client.UserId, cancellationToken);
        }
    }
}