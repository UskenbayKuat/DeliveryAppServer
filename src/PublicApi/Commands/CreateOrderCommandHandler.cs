using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using AutoMapper;
using BackgroundTasks.Interfaces;
using BackgroundTasks.Model;
using MediatR;
using PublicApi.Helpers;

namespace PublicApi.Commands
{
    public class CreateOrderCommandHandler : AsyncRequestHandler<CreateOrderCommand>
    {
        private readonly IOrder _order;
        private readonly IDelivery _delivery;
        private readonly IMapper _mapper;
        private readonly IChatHub _chatHub;
        private readonly HubHelper _hubHelper;
        private readonly IBackgroundTaskQueue _backgroundTask;

        public CreateOrderCommandHandler(IOrder order, IMapper mapper, IChatHub chatHub, IDelivery delivery, HubHelper hubHelper, IBackgroundTaskQueue backgroundTask)
        {
            _order = order;
            _mapper = mapper;
            _chatHub = chatHub;
            _delivery = delivery;
            _hubHelper = hubHelper;
            _backgroundTask = backgroundTask;
        }

        protected override async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _order.CreateAsync(_mapper.Map<OrderInfo>(request), request.UserId, cancellationToken);
            var delivery = await _delivery.FindIsActiveDelivery(order, cancellationToken);
            if (delivery is null)
            {
                return;
            }
            await _backgroundTask.QueueAsync(new BackgroundOrder(order.Id, delivery.Id));
            await _order.UpdateOrderAsync(order, delivery, (int)GeneralState.OnReview);
            
            var connectionDriverId = await _chatHub.GetConnectionIdAsync(delivery.RouteTrip.Driver.UserId, cancellationToken);
            await _hubHelper.SendToDriverAsync(connectionDriverId, cancellationToken);
        }
    }
}