using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using MediatR;
using Notification.Interfaces;

namespace PublicApi.Commands
{
    public class RejectedOrderCommandHandler : AsyncRequestHandler<RejectedOrderCommand>
    {
        private readonly IOrderHandler _orderHandler;
        private readonly INotify _notify;

        public RejectedOrderCommandHandler(INotify notify, IOrderHandler orderHandler)
        {
            _notify = notify;
            _orderHandler = orderHandler;
        }

        protected override async Task Handle(RejectedOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderHandler.RejectedHandlerAsync(request.OrderId, cancellationToken);
            await _notify.SendToDriverAsync(order.Delivery.Driver.UserId, cancellationToken);
        }
    }
}