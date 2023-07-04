using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using MediatR;
using Notification.Interfaces;

namespace PublicApi.Commands.Orders.Handlers
{
    public class ConfirmOrderCommandHandler : AsyncRequestHandler<ConfirmOrderCommand>
    {
        private readonly INotify _notify;
        private readonly IOrderCommand _orderCommand;

        public ConfirmOrderCommandHandler(INotify notify, IOrderCommand orderCommand)
        {
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