using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PublicApi.Endpoints.Delivery.Query
{
    [Authorize]
    public class GetActiveDelivery : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryQuery _deliveryQuery;
        private readonly ILogger<GetActiveDelivery> _logger;
        public GetActiveDelivery(IDeliveryQuery deliveryQuery, ILogger<GetActiveDelivery> logger)
        {
            _deliveryQuery = deliveryQuery;
            _logger = logger;
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
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest("Ошибка системы");
            }
        }
    }
}