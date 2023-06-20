using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery
{
    [Authorize]
    public class GetActiveOrdersForClient: EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IOrderQuery _orderQuery;

        public GetActiveOrdersForClient(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        [HttpPost("api/client/activeOrders")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default) =>
            await _orderQuery.GetActiveOrdersForClientAsync(HttpContext.Items["UserId"]?.ToString());
    }

}