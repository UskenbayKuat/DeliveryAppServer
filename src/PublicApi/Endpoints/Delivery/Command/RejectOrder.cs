using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;
using PublicApi.Commands.Deliveries.Models;

namespace PublicApi.Endpoints.Delivery.Command
{
    public class RejectOrder : EndpointBaseAsync.WithRequest<RejectedOrderCommand>.WithActionResult
    {
        private readonly IMediator _mediator;

        public RejectOrder(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/driver/rejectOrder")]
        public override async Task<ActionResult> HandleAsync([FromBody] RejectedOrderCommand request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _mediator.Send(request, cancellationToken);
                return new NoContentResult();
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