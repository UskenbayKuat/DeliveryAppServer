using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using MediatR;
using PublicApi.Helpers;

namespace PublicApi.Commands
{
    public class RejectedOrderCommandHandler : AsyncRequestHandler<RejectedOrderCommand>
    {
        private readonly IOrderHandler _orderHandler;
        private readonly HubHelper _hubHelper;

        public RejectedOrderCommandHandler(HubHelper hubHelper, IOrderHandler orderHandler)
        {
            _hubHelper = hubHelper;
            _orderHandler = orderHandler;
        }

        protected override async Task Handle(RejectedOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderHandler.RejectedHandlerAsync(request.OrderId, cancellationToken);
            await _hubHelper.SendToDriverAsync(order.Delivery.RouteTrip.Driver.UserId, cancellationToken);
        }
    }
}