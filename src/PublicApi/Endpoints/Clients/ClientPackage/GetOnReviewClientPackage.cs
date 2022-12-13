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
        private readonly IOrder _order;

        public GetOnReviewClientPackage(IOrder order)
        {
            _order = order;
        }

        [HttpPost("api/clients/OnReviewClientPackage")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _order.GetOnReviewOrdersAsync(HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }
}