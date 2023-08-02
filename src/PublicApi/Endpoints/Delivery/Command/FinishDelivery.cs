using ApplicationCore.Interfaces.DeliveryInterfaces;
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

        [HttpPost("api/driver/rejectOrder")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _deliveryCommand.FinishAsync(HttpContext.Items["UserId"].ToString());
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
