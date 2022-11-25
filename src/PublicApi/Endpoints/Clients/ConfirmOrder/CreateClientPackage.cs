using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.OrderInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicApi.Hub;

namespace PublicApi.Endpoints.Clients.ConfirmOrder
{
    public class CreateClientPackage : EndpointBaseAsync.WithRequest<ClientPackageCommand>.WithActionResult
    {
        private readonly IClientPackage _clientPackage;
        private readonly IOrder _order;
        private readonly IMapper _mapper;
        private readonly IHubContext<Notification> _hubContext;

        public CreateClientPackage(IClientPackage clientPackage, IMapper mapper, IHubContext<Notification> hubContext, IOrder order)
        {
            _clientPackage = clientPackage;
            _mapper = mapper;
            _hubContext = hubContext;
            _order = order;
        }

        [HttpPost("api/client/confirmClientPackage")]
        public override async Task<ActionResult> HandleAsync([FromBody]ClientPackageCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var orderInfo = await _clientPackage.CreateAsync(_mapper.Map<ClientPackageInfo>(request), HttpContext.Items["UserId"]?.ToString(), cancellationToken);
                var driverConnectId = await _order.FindDriverConnectionIdAsync(orderInfo, cancellationToken);
                if (!string.IsNullOrEmpty(driverConnectId))
                    await _hubContext.Clients.Client(driverConnectId)
                        .SendCoreAsync("SendClientInfoToDriver", new[] {  new List<ClientPackageInfoToDriver>{orderInfo}}, cancellationToken);
                return Ok(request);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}