using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery.Command
{
    public class CancelDelivery : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryCommand _deliveryCommand;

        public CancelDelivery(IDeliveryCommand deliveryCommand)
        {
            _deliveryCommand = deliveryCommand;
        }

        [HttpPost("api/driver/cancelDelivery")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _deliveryCommand.CancellationAsync(HttpContext.Items["UserId"].ToString());
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