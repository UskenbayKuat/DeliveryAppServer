using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery.Query
{
    [Authorize]
    public class GetActiveDelivery : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryQuery _deliveryQuery;

        public GetActiveDelivery(IDeliveryQuery deliveryQuery)
        {
            _deliveryQuery = deliveryQuery;
        }
        
        [HttpPost("api/driver/activeDelivery")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var deliveryDto =
                    await _deliveryQuery.GetDeliveryIsActiveAsync(HttpContext.Items["UserId"]?.ToString());
                return Ok(deliveryDto);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }
        }
    }
}