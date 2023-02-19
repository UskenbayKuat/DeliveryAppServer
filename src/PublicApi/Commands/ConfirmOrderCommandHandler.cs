using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using MediatR;
using PublicApi.Helpers;

namespace PublicApi.Commands
{
    public class ConfirmOrderCommandHandler : AsyncRequestHandler<ConfirmOrderCommand>
    {
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly HubHelper _hubHelper;


        public ConfirmOrderCommandHandler(IDeliveryCommand deliveryCommand, HubHelper hubHelper)
        {
            _deliveryCommand = deliveryCommand;
            _hubHelper = hubHelper;
        }

        protected override async Task Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order  = await _deliveryCommand.AddOrderAsync(request.OrderId);
            await _hubHelper.SendToClient(order.Client.UserId, cancellationToken);
        }
    }
}