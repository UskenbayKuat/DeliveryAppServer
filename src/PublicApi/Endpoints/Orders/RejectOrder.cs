using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DriverInterfaces;
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
        private readonly IDriver _driverService;
        private readonly IHubContext<Notification> _hubContext;
        private readonly IMapper _mapper;

        public RejectOrder(IDriver driverService, IHubContext<Notification> hubContext, IMapper mapper)
        {
            _driverService = driverService;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpPost("api/rejectOrder")]
        public override async Task<ActionResult> HandleAsync(OrderCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var clientPackageInfo = _mapper.Map<ClientPackageInfo>(request);
                var driverConnectId =  await _driverService.RejectNextFindDriverConnectionIdAsync(HttpContext.Items["UserId"]?.ToString(), clientPackageInfo, cancellationToken);
                if (!string.IsNullOrEmpty(driverConnectId))
                    await _hubContext.Clients.User(driverConnectId)
                        .SendCoreAsync("SendClientInfoToDriver", new[] { new List<ClientPackageInfo>{clientPackageInfo} }, cancellationToken);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}