using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;
using PublicApi.Commands.Deliveries.Models;

namespace PublicApi.Endpoints.Delivery
{
    [Authorize]
    public class ConfirmOrder : EndpointBaseAsync.WithRequest<ConfirmOrderCommand>.WithActionResult
    {
        private readonly IMediator _mediator;

        public ConfirmOrder(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/driver/confirmOrder")]
        public override async Task<ActionResult> HandleAsync([FromBody]ConfirmOrderCommand request,
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(request, cancellationToken);
            return new NoContentResult();
        }
    }
}