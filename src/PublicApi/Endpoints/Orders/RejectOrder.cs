using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.OrderInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PublicApi.Hub;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PublicApi.Endpoints.Orders
{
    public class RejectOrder : EndpointBaseAsync.WithRequest<OrderCommand>.WithActionResult
    {
        private readonly IOrder _order;
        private readonly IHubContext<Notification> _hubContext;
        private readonly IMapper _mapper;

        public RejectOrder(IOrder order, IHubContext<Notification> hubContext, IMapper mapper)
        {
            _order = order;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpPost("api/rejectOrder")]
        public override async Task<ActionResult> HandleAsync(OrderCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var orderInfo = _mapper.Map<ClientPackageInfoToDriver>(request);
                var driverConnectId =  await _order.RejectAsync(HttpContext.Items["UserId"]?.ToString(), orderInfo, cancellationToken);
                if (!string.IsNullOrEmpty(driverConnectId))
                    await _hubContext.Clients.User(driverConnectId)
                        .SendCoreAsync("SendClientInfoToDriver", new[] { new List<ClientPackageInfoToDriver>{orderInfo} }, cancellationToken);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}