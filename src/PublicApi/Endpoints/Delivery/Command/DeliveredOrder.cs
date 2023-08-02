using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Deliveries;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace PublicApi.Endpoints.Delivery.Command
{
    public class DeliveredOrder : EndpointBaseAsync.WithRequest<DeliveredOrderCommand>.WithActionResult
    {
        private readonly IMediator _mediator;
        public DeliveredOrder(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/driver/clientDelivered")]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] DeliveredOrderCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                await _mediator.Send(request.SetUserId(HttpContext.Items["UserId"].ToString()), cancellationToken);
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
