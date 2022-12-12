using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Clients.ClientPackage
{
    [Authorize]
    public class GetOnReviewClientPackage : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IClientPackage _clientPackage;

        public GetOnReviewClientPackage(IClientPackage clientPackage)
        {
            _clientPackage = clientPackage;
        }

        [HttpPost("api/clients/OnReviewClientPackage")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _clientPackage.GetOnReviewClientPackageAsync(HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }
}