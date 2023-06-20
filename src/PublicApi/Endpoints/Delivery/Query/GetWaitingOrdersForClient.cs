using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery
{
    [Authorize]
    public class GetWaitingOrdersForClient : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IOrderQuery _orderQuery;

        public GetWaitingOrdersForClient(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        [HttpPost("api/client/waitingOrders")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var orders = await _orderQuery.GetWaitingOrdersAsync(HttpContext.Items["UserId"]?.ToString());
                return Ok(orders);
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }        
        }
    }
}