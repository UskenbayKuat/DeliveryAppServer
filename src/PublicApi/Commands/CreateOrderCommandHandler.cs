using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.Values;
using AutoMapper;
using MediatR;
using PublicApi.Helpers;

namespace PublicApi.Commands
{
    public class CreateOrderCommandHandler : AsyncRequestHandler<CreateOrderCommand>
    {
        private readonly IOrderHandler _orderHandler;
        private readonly IMapper _mapper;
        private readonly HubHelper _hubHelper;

        public CreateOrderCommandHandler(IMapper mapper, HubHelper hubHelper, IOrderHandler orderHandler)
        {
            _mapper = mapper;
            _hubHelper = hubHelper;
            _orderHandler = orderHandler;
        }

        protected override async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderHandler.CreatedHandlerAsync(_mapper.Map<OrderInfo>(request), request.UserId,
                cancellationToken);
            order = await _orderHandler.FindIsNewDeliveryHandlerAsync(order, cancellationToken);
            await _hubHelper.SendToDriverAsync(order.Delivery?.Driver.UserId, cancellationToken);
        }
    }
}