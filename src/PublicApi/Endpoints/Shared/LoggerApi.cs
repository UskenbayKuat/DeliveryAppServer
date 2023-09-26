using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Endpoints.Shared
{
    [Authorize]
    public class LoggerApi : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly 

        [HttpPost("api/logger")]
        public async override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return NoContent();
        }
    }
}
