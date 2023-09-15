using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Orders.Models;

namespace PublicApi.Endpoints.Clients.Command
{
    [Authorize]
    public class CreateOrder : EndpointBaseAsync.WithRequest<CreateOrderCommand>.WithActionResult
    {
        private readonly IMediator _mediator;
        public CreateOrder(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/client/createOrder")]
        public override async Task<ActionResult> HandleAsync([FromBody] CreateOrderCommand request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _mediator.Send(request.SetUserId(HttpContext.Items["UserId"]?.ToString()), cancellationToken);
                return new NoContentResult();
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }
        }
    }
}