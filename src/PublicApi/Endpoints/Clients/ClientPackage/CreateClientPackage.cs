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

namespace PublicApi.Endpoints.Clients.ClientPackage
{
    [Authorize]
    public class CreateClientPackage : EndpointBaseAsync.WithRequest<ClientPackageCommand>.WithActionResult
    {
        private readonly IClientPackage _clientPackage;
        private readonly IDriver _driverService;
        private readonly IMapper _mapper;
        private readonly IHubContext<Notification> _hubContext;

        public CreateClientPackage(IClientPackage clientPackage, IMapper mapper, IHubContext<Notification> hubContext, IDriver driverService)
        {
            _clientPackage = clientPackage;
            _mapper = mapper;
            _hubContext = hubContext;
            _driverService = driverService;
        }

        [HttpPost("api/client/confirmClientPackage")]
        public override async Task<ActionResult> HandleAsync([FromBody]ClientPackageCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var clientPackageInfo = await _clientPackage.CreateAsync(_mapper.Map<ClientPackageInfo>(request), HttpContext.Items["UserId"]?.ToString(), cancellationToken);
                var driverConnectId = await _driverService.FindDriverConnectionIdAsync(clientPackageInfo, cancellationToken);
                if (!string.IsNullOrEmpty(driverConnectId))
                    await _hubContext.Clients.Client(driverConnectId)
                        .SendCoreAsync("SendClientInfoToDriver", new[] {  new List<ClientPackageInfo>{clientPackageInfo}}, cancellationToken);
                return Ok(request);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}