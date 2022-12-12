using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Clients.ClientPackage
{
    [Authorize]
    public class GetWaitingClientPackage : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IClientPackage _clientPackage;

        public GetWaitingClientPackage(IClientPackage clientPackage)
        {
            _clientPackage = clientPackage;
        }

        [HttpPost("api/clients/WaitingClientPackage")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _clientPackage.GetWaitingClientPackageAsync(HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }
}