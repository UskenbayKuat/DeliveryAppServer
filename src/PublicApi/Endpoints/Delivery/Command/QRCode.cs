using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Deliveries.Models;

namespace PublicApi.Endpoints.Delivery.Command
{
    public class QRCode : EndpointBaseAsync.WithRequest<QRCodeCommand>.WithActionResult
    {
        private readonly IMediator _mediator;

        public QRCode(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/driver/qrCode")]
        public override async Task<ActionResult> HandleAsync([FromBody] QRCodeCommand request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _mediator.Send(request.SetUserId(HttpContext.Items["UserId"]?.ToString()), cancellationToken);
                return new NoContentResult();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }
        }
            
    }
}