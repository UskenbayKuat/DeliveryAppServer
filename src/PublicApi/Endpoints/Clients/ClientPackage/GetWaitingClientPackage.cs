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
        private readonly IOrder _order;

        public GetWaitingClientPackage(IOrder order)
        {
            _order = order;
        }

        [HttpPost("api/clients/WaitingClientPackage")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _order.GetWaitingOrdersAsync(HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }
}