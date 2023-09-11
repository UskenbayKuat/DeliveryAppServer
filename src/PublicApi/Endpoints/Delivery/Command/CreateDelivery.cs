#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;
using PublicApi.Commands.Deliveries.Models;

namespace PublicApi.Endpoints.Delivery.Command
{
    [Authorize]
    public class CreateDelivery : EndpointBaseAsync.WithRequest<CreateDeliveryCommand>.WithActionResult
    {
        private readonly IMediator _mediator;

        public CreateDelivery(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/driver/createDelivery")]
        public override async Task<ActionResult> HandleAsync([FromBody] CreateDeliveryCommand request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _mediator.Send(request.SetUserId(HttpContext.Items["UserId"]?.ToString()), cancellationToken);
                return new NoContentResult();
            }
            catch(CarNotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ArgumentException ex)
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