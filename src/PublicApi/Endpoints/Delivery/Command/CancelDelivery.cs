using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Drivers;
using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery.Command
{
    [Authorize]
    public class CancelDelivery : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryCommand _deliveryCommand;

        public CancelDelivery(IDeliveryCommand deliveryCommand)
        {
            _deliveryCommand = deliveryCommand;
        }

        [HttpPost("api/cancelDelivery")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var driverUserId = HttpContext.Items["UserId"].ToString();
                await _deliveryCommand.CancellationAsync(Guid.Parse(driverUserId));
                return new NoContentResult();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Ошибка");
            }
        }
    }
}