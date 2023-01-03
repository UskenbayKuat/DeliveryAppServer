#nullable enable
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;
using PublicApi.Helpers;

namespace PublicApi.Endpoints.Drivers.RouteTrip
{
    [Authorize]
    public class CreateRouteTrip : EndpointBaseAsync.WithRequest<CreateDeliveryCommand>.WithActionResult
    {
        private readonly IMediator _mediator;
        private readonly HubHelper _hubHelper;

        public CreateRouteTrip(IMediator mediator, HubHelper hubHelper)
        {
            _mediator = mediator;
            _hubHelper = hubHelper;
        }

        [HttpPost("api/driver/createRouteTrip")]
        public override async Task<ActionResult> HandleAsync([FromBody] CreateDeliveryCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var driverConnectionId =
                    await _mediator.Send(request.SetUserId(HttpContext.Items["UserId"]?.ToString()), cancellationToken);
                await _hubHelper.SendToClient(driverConnectionId, cancellationToken);
                return new NoContentResult();
            }
            catch
            {
                return new BadRequestObjectResult("Сначала завершите текущий маршрут");
            }
        }
    }
}