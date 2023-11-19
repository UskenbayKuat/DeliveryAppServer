using ApplicationCore.Interfaces.Drivers;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Deliveries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Endpoints.Delivery.Command
{
    public class FinishDelivery : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryCommand _deliveryCommand;

        public FinishDelivery(IDeliveryCommand deliveryCommand)
        {
            _deliveryCommand = deliveryCommand;
        }

        [HttpPost("api/finishDelivery")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var driverUserId = HttpContext.Items["UserId"].ToString();
                await _deliveryCommand.FinishAsync(Guid.Parse(driverUserId));
                return new NoContentResult();
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
