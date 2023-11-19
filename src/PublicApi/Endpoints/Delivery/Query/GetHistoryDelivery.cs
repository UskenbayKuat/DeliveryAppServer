using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using Infrastructure.Config;
using ApplicationCore.Interfaces.Drivers;

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

        [HttpPost("api/historyDelivery")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var driverUserId = HttpContext.Items["UserId"].ToString();
                var deliveryDtoList =
                    await _deliveryQuery.GetHistoryAsync(Guid.Parse(driverUserId));
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