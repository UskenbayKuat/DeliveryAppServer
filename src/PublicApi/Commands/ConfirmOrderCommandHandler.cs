using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using MediatR;
using Notification.Interfaces;

namespace PublicApi.Commands
{
    public class ConfirmOrderCommandHandler : AsyncRequestHandler<ConfirmOrderCommand>
    {
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly INotify _notify;
        private readonly IOrderCommand _orderCommand;

        public ConfirmOrderCommandHandler(IDeliveryCommand deliveryCommand, INotify notify, IOrderCommand orderCommand)
        {
            _deliveryCommand = deliveryCommand;
            _notify = notify;
            _orderCommand = orderCommand;
        }

        protected override async Task Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order  = await _orderCommand.UpdateStatePendingAsync(request.OrderId);
            await _notify.SendToClient(order.Client.UserId, cancellationToken);
        }
    }
}