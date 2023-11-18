using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Drivers;
using Ardalis.ApiEndpoints;
using Infrastructure.Config;
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
                var driverUserId = HttpContext.Items["UserId"].ToString();
                var deliveryDto =
                    await _deliveryQuery.GetDeliveryIsActiveAsync(Guid.Parse(driverUserId));
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