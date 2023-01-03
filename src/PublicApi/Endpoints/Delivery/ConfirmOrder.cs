using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;
using PublicApi.Helpers;

namespace PublicApi.Endpoints.Delivery
{
    [Authorize]
    public class ConfirmOrder : EndpointBaseAsync.WithRequest<ConfirmOrderCommand>.WithActionResult
    {
        private readonly HubHelper _hubHelper;
        private readonly IMediator _mediator;

        public ConfirmOrder(IMediator mediator, HubHelper hubHelper)
        {
            _mediator = mediator;
            _hubHelper = hubHelper;
        }

        [HttpPost("api/driver/confirmOrder")]
        public override async Task<ActionResult> HandleAsync([FromBody]ConfirmOrderCommand request,
            CancellationToken cancellationToken = default)
        {
            var clientConnectionId = await _mediator.Send(request, cancellationToken);
            await _hubHelper.SendToClient(clientConnectionId, cancellationToken);
            return new NoContentResult();
        }
    }
}