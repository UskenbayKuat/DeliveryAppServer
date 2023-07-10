using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using MediatR;
using Notification.Interfaces;

namespace PublicApi.Commands.Deliveries.Handlers
{
    public class RejectedOrderCommandHandler : AsyncRequestHandler<RejectedOrderCommand>
    {
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly IOrderCommand _orderCommand;
        private readonly INotify _notify;

        public RejectedOrderCommandHandler(INotify notify, IDeliveryCommand deliveryCommand, IOrderCommand orderCommand)
        {
            _notify = notify;
            _deliveryCommand = deliveryCommand;
            _orderCommand = orderCommand;
        }

        protected override async Task Handle(RejectedOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderCommand.RejectAsync(request.OrderId) 
                        ?? throw new ArgumentException("Ошибка, заказ передан");
            var delivery = await _deliveryCommand.FindIsNewDeliveryAsync(order);
            if (delivery != null)
            {
                await _orderCommand.SetDeliveryAsync(order, delivery);
                await _notify.SendToDriverAsync(delivery.Driver.UserId, cancellationToken);
            }
        }
    }
}