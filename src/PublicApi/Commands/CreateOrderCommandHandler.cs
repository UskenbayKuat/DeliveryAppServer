using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.Values;
using AutoMapper;
using MediatR;
using Notification.Interfaces;

namespace PublicApi.Commands
{
    public class CreateOrderCommandHandler : AsyncRequestHandler<CreateOrderCommand>
    {
        private readonly IOrderHandler _orderHandler;
        private readonly IMapper _mapper;
        private readonly INotify _notify;

        public CreateOrderCommandHandler(IMapper mapper, INotify notify, IOrderHandler orderHandler)
        {
            _mapper = mapper;
            _notify = notify;
            _orderHandler = orderHandler;
        }

        protected override async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderHandler.CreatedHandlerAsync(_mapper.Map<OrderInfo>(request), request.UserId,
                cancellationToken);
            order = await _orderHandler.FindIsNewDeliveryHandlerAsync(order, cancellationToken);
            await _notify.SendToDriverAsync(order.Delivery?.Driver.UserId, cancellationToken);
        }
    }
}