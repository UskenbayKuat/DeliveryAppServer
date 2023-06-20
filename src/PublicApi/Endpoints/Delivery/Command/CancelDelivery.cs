using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery
{
    public class CancelDelivery : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryCommand _deliveryCommand;

        public CancelDelivery(IDeliveryCommand deliveryCommand)
        {
            _deliveryCommand = deliveryCommand;
        }

        [HttpPost("api/driver/cancelDelivery")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await _deliveryCommand.CancellationAsync(HttpContext.Items["UserId"].ToString());
        }
    }
}