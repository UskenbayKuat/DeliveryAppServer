using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Orders;

namespace PublicApi.Endpoints.Clients.Command
{
    public class CancelOrder : EndpointBaseAsync.WithRequest<CancelOrderCommand>.WithActionResult
    {
        private readonly IOrderCommand _orderCommand;

        public CancelOrder(IOrderCommand orderCommand)
        {
            _orderCommand = orderCommand;
        }
        [HttpPost("api/client/cancelOrder")]
        public override async Task<ActionResult> HandleAsync([FromBody]CancelOrderCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await _orderCommand.CancelAsync(request.OrderId);
                return new NoContentResult();
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