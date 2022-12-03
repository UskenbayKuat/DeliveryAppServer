using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.OrderInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Orders
{
    [Authorize]
    public class GetOrder: EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IOrder _order;

        public GetOrder(IOrder order)
        {
            _order = order;
        }

        [HttpPost("api/ClientActiveOrder")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _order.GetActiveOrdersForClient(HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }

}