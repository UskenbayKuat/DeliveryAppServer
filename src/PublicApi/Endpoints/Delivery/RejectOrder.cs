using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;
using PublicApi.Helpers;

namespace PublicApi.Endpoints.Delivery
{
    public class RejectOrder : EndpointBaseAsync.WithRequest<RejectedOrderCommand>.WithActionResult
    {
        private readonly IMediator _mediator;
        private readonly HubHelper _hubHelper;

        public RejectOrder(IMediator mediator, HubHelper hubHelper)
        {
            _mediator = mediator;
            _hubHelper = hubHelper;
        }

        [HttpPost("api/driver/rejectOrder")]
        public override async Task<ActionResult> HandleAsync([FromBody] RejectedOrderCommand request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var driverConnectionId =
                    await _mediator.Send(request.SetUserId(HttpContext.Items["UserId"]?.ToString()), cancellationToken);
                await _hubHelper.SendToDriver(driverConnectionId, cancellationToken);
                return new NoContentResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}