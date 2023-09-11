using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using ApplicationCore.Models.Dtos.Deliveries;
using AutoMapper;
using ApplicationCore.Interfaces.ClientInterfaces;
using MediatR;
using PublicApi.Commands.Deliveries.Models;

namespace PublicApi.Endpoints.Delivery.Command
{
    /// <summary>
    /// Прибыль клиента
    /// </summary>
    public class ProfitOrder : EndpointBaseAsync.WithRequest<ProfitOrderCommand>.WithActionResult
    {
        private readonly IMediator _mediator;
        public ProfitOrder(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/driver/profitOrder")]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] ProfitOrderCommand request, CancellationToken cancellationToken = default)
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
