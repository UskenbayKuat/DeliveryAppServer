using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery
{
    [Authorize]
    public class GetInProgressOrdersForClient: EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDelivery _delivery;

        public GetInProgressOrdersForClient(IDelivery delivery)
        {
            _delivery = delivery;
        }

        [HttpPost("api/ClientActiveOrder")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _delivery.GetInProgressOrdersForClientAsync(HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }

}