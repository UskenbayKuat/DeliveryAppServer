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
    public class RejectedOrderCommandHandler : AsyncRequestHandler<RejectedOrderCommand>
    {
        private readonly IDriver _driverService;
        private readonly IChatHub _chatHub;
        private readonly IDelivery _delivery;
        private readonly IBackgroundTaskQueue _backgroundTask;
        private readonly IOrder _order;
        private readonly HubHelper _hubHelper;

        public RejectedOrderCommandHandler(IDriver driverService, IChatHub chatHub, IDelivery delivery, IBackgroundTaskQueue backgroundTask, IOrder order, HubHelper hubHelper)
        {
            _driverService = driverService;
            _chatHub = chatHub;
            _delivery = delivery;
            _backgroundTask = backgroundTask;
            _order = order;
            _hubHelper = hubHelper;
        }

        protected override async Task Handle(RejectedOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _driverService.RejectOrderAsync(request.OrderId);
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