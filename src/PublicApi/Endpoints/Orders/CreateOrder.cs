using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.OrderInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicApi.HubNotification;

namespace PublicApi.Endpoints.Orders
{
    public class CreateOrder : EndpointBaseAsync.WithRequest<OrderCommand>.WithActionResult
    {
        private readonly IOrder _order;
        private readonly IHubContext<Notification> _hubContext;

        public CreateOrder(IOrder order, IHubContext<Notification> hubContext)
        {
            _order = order;
            _hubContext = hubContext;
        }
        [HttpPost("api/Order")]
        public override async Task<ActionResult> HandleAsync(OrderCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var clientConnectId =await _order.CreateAsync(HttpContext.Items["UserId"]?.ToString() , request.ClientPackageId);
            if (!string.IsNullOrEmpty(clientConnectId))
                await _hubContext.Clients.User(clientConnectId)
                    .SendCoreAsync("SendDriverInfoToClient", new[] { "Ваш заказ принята" }, cancellationToken);
            return Ok();
        }
    }
}