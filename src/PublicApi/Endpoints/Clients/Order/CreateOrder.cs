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
        private readonly IDriver _driverService;
        private readonly IMapper _mapper;
        private readonly IHubContext<Notification> _hubContext;

        public CreateOrder(IOrder order, IMapper mapper, IHubContext<Notification> hubContext, IDriver driverService)
        {
            _order = order;
            _mapper = mapper;
            _hubContext = hubContext;
            _driverService = driverService;
        }

        [HttpPost("api/client/confirmClientPackage")]
        public override async Task<ActionResult> HandleAsync([FromBody] OrderCommand request,
            CancellationToken cancellationToken = new CancellationToken())
            => await _order.CreateAsync(_mapper.Map<OrderInfo>(request),
                    HttpContext.Items["UserId"]?.ToString(), 
                    async (connectionId, info) => await _hubContext.Clients
                        .Client(connectionId)
                        .SendCoreAsync("SendClientInfoToDriver", new[] { new List<OrderInfo> { info } },
                            cancellationToken), 
                    cancellationToken);
    }
}