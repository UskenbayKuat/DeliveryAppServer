using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Orders
{
    [Authorize]
    public class GetOrder: EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDelivery _delivery;

        public GetOrder(IDelivery delivery)
        {
            _delivery = delivery;
        }

        [HttpPost("api/ClientActiveOrder")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _delivery.GetActiveDeliveriesForClient(HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }

}