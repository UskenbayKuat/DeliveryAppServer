using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicApi.Endpoints.Delivery;
using PublicApi.HubNotification;

namespace PublicApi.Endpoints.Orders
{
    public class AddOrderToDelivery : EndpointBaseAsync.WithRequest<DeliveryCommand>.WithActionResult
    {
        private readonly IDelivery _delivery;
        private readonly IHubContext<Notification> _hubContext;

        public AddOrderToDelivery(IDelivery delivery, IHubContext<Notification> hubContext)
        {
            _delivery = delivery;
            _hubContext = hubContext;
        }

        [HttpPost("api/Order")]
        public override async Task<ActionResult> HandleAsync(DeliveryCommand request,
            CancellationToken cancellationToken = new CancellationToken()) =>
            await _delivery.AddToDeliveryAsync(request.ClientPackageId, async clientConnectId => await _hubContext
                .Clients.User(clientConnectId)
                .SendCoreAsync("SendDriverInfoToClient", new[] { "Ваш заказ принята" }, cancellationToken));
    }
}