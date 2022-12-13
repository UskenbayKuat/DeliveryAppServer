using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicApi.HubNotification;

namespace PublicApi.Endpoints.Orders
{
    public class CreateOrder : EndpointBaseAsync.WithRequest<OrderCommand>.WithActionResult
    {
        private readonly IDelivery _delivery;
        private readonly IHubContext<Notification> _hubContext;

        public CreateOrder(IDelivery delivery, IHubContext<Notification> hubContext)
        {
            _delivery = delivery;
            _hubContext = hubContext;
        }
        [HttpPost("api/Order")]
        public override async Task<ActionResult> HandleAsync(OrderCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var clientConnectId =await _delivery.AddToDeliveryAsync(request.ClientPackageId);
            if (!string.IsNullOrEmpty(clientConnectId))
                await _hubContext.Clients.User(clientConnectId)
                    .SendCoreAsync("SendDriverInfoToClient", new[] { "Ваш заказ принята" }, cancellationToken);
            return Ok();
        }
    }
}