using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;

namespace PublicApi.Endpoints.Delivery.Command
{
    public class StartDelivery : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IMediator _mediator;

        public StartDelivery(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("api/driver/startDelivery")]

        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
        {
            try
            {
                await _mediator.Send(new StartDeliveryCommand(HttpContext.Items["UserId"].ToString()),
                    cancellationToken);
                return new NoContentResult();
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }
        }
    }
}