using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery.Query
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
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var deliveryInfos = await _orderQuery.GetActiveOrdersForClientAsync(HttpContext.Items["UserId"]?.ToString());
                return Ok(deliveryInfos);
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }
        }
    }

}