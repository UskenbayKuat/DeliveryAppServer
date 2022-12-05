using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Clients.ClientPackage
{
    [Authorize]
    public class GetClientPackage : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IClientPackage _clientPackage;

        public GetClientPackage(IClientPackage clientPackage)
        {
            _clientPackage = clientPackage;
        }

        [HttpPost("api/WaitingClientPackage")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _clientPackage.GetWaitingClientPackage(HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }
}