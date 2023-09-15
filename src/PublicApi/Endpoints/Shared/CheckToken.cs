using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Endpoints.Shared
{
    [Authorize]
    public class CheckToken : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        [HttpPost("api/checkToken")]
        public async override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return NoContent();
        }
    }
}
