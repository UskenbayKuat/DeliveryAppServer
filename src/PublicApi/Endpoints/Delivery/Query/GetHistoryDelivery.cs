using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using Infrastructure.Config.Attributes;

namespace PublicApi.Endpoints.Delivery.Query
{
    [Authorize]
    public class GetHistoryDelivery : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryQuery _deliveryQuery;

        public GetHistoryDelivery(IDeliveryQuery deliveryQuery)
        {
            _deliveryQuery = deliveryQuery;
        }

        [HttpPost("api/driver/historyDelivery")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var deliveryDtoList =
                    await _deliveryQuery.GetHistoryAsync(HttpContext.Items["UserId"]?.ToString());
                return Ok(deliveryDtoList);
            }
            catch (ArgumentException ex)
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