using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Clients;
using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Clients.Query
{
    [Authorize]
    public class GetActiveOrders: EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IOrderQuery _orderQuery;

        public GetActiveOrders(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        [HttpPost("api/client/activeOrders")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var deliveryInfos = 
                    await _orderQuery.GetActiveOrdersForClientAsync(HttpContext.Items["UserId"]?.ToString());
                return Ok(deliveryInfos);
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }
        }
    }

}