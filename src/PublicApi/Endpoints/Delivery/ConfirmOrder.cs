using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicApi.HubNotification;

namespace PublicApi.Endpoints.Delivery
{
    public class ConfirmOrder : EndpointBaseAsync.WithRequest<DeliveryCommand>.WithActionResult
    {
        private readonly IDelivery _delivery;
        private readonly IHubContext<Notification> _hubContext;

        public ConfirmOrder(IDelivery delivery, IHubContext<Notification> hubContext)
        {
            _delivery = delivery;
            _hubContext = hubContext;
        }

        [HttpPost("api/driver/confirmOrder")]
        public override async Task<ActionResult> HandleAsync([FromBody]DeliveryCommand request,
            CancellationToken cancellationToken = default) =>
            await _delivery.AddToDeliveryAsync(request.OrderId, 
                async (connectionId) => await SendInfoForClientAsync(connectionId));

        private async Task SendInfoForClientAsync(string clientConnectId) =>
            await _hubContext.Clients.Client(clientConnectId).
                SendCoreAsync("SendToClient", new[] { "Ваш заказ принят, ожидает передачи" });
    }
}