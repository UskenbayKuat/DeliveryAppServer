using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicApi.HubNotification;

namespace PublicApi.Endpoints.Clients.Order
{
    [Authorize]
    public class CreateOrder : EndpointBaseAsync.WithRequest<OrderCommand>.WithActionResult
    {
        private readonly IOrder _order;
        private readonly IMapper _mapper;
        private readonly IHubContext<Notification> _hubContext;

        public CreateOrder(IOrder order, IMapper mapper, IHubContext<Notification> hubContext)
        {
            _order = order;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpPost("api/client/confirmClientPackage")]
        public override async Task<ActionResult> HandleAsync([FromBody] OrderCommand request,
            CancellationToken cancellationToken = default) =>
            await _order.CreateAsync(
                info: _mapper.Map<OrderInfo>(request),
                clientUserId: HttpContext.Items["UserId"]?.ToString(),
                func: SendInfoForClientAsync, cancellationToken);

        private async Task SendInfoForClientAsync(string connectionId, OrderInfo info) =>
            await _hubContext.Clients.User(connectionId)
                .SendCoreAsync("SendClientInfoToDriver", new[] { new List<OrderInfo> { info } });
    }
}