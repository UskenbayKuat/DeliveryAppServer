using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Models.Dtos;
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
        private readonly IOrderCommand _orderCommand;


        public CreateOrderCommandHandler(IMapper mapper, INotify notify, IOrderHandler orderHandler, IOrderCommand orderCommand)
        {
            _mapper = mapper;
            _notify = notify;
            _orderHandler = orderHandler;
            _orderCommand = orderCommand;
        }

        protected override async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderCommand.CreateAsync(_mapper.Map<CreateOrderDto>(request), request.UserId, cancellationToken);
            order = await _orderHandler.FindIsNewDeliveryHandlerAsync(order, cancellationToken);
            await _notify.SendToDriverAsync(order.Delivery?.Driver.UserId, cancellationToken);
        }
    }
}