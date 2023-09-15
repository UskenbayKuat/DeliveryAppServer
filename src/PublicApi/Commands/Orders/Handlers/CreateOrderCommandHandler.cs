using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Interfaces.Drivers;
using ApplicationCore.Models.Dtos.Orders;
using AutoMapper;
using MediatR;
using Notification.Services;
using PublicApi.Commands.Orders.Models;

namespace PublicApi.Commands.Orders.Handlers
{
    public class CreateOrderCommandHandler : AsyncRequestHandler<CreateOrderCommand>
    {
        private readonly IDeliveryCommand _deliveryCommand;
        private readonly IMapper _mapper;
        private readonly INotify _notify;
        private readonly IOrderCommand _orderCommand;


        public CreateOrderCommandHandler(IMapper mapper, INotify notify, IOrderCommand orderCommand, IDeliveryCommand deliveryCommand)
        {
            _mapper = mapper;
            _notify = notify;
            _orderCommand = orderCommand;
            _deliveryCommand = deliveryCommand;
        }

        protected override async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderCommand.CreateAsync(_mapper.Map<CreateOrderDto>(request), request.UserId);
            var delivery = await _deliveryCommand.FindIsActiveDeliveryAsync(order);
            if (delivery != null)
            {
                await _orderCommand.SetDeliveryAsync(order, delivery);
                await _notify.SendToDriverAsync(delivery.Driver.UserId, cancellationToken);
            }
        }
    }
}