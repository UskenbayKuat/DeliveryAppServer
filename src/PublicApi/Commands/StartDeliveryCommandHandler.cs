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
        private readonly IOrderQuery _orderQuery;
        private readonly INotify _notify;

        public StartDeliveryCommandHandler(IDeliveryCommand deliveryCommand, IOrderQuery orderQuery, INotify notify)
        {
            _deliveryCommand = deliveryCommand;
            _orderQuery = orderQuery;
            _notify = notify;
        }

        public async Task<ActionResult> Handle(StartDeliveryCommand request, CancellationToken cancellationToken)
        {
            await _deliveryCommand.StartAsync(request.UserId);
            var orders = await _orderQuery.GetOrdersAsync(request.UserId);
            await _notify.SendInfoToClientsAsync(orders);
            return new NoContentResult();
        }
    }
}