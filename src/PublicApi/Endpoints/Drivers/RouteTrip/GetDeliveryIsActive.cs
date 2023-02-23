using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.RouteTrip
{
    [Authorize]
    public class GetDeliveryIsActive : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryQuery _deliveryQuery;

        public GetDeliveryIsActive( IDeliveryQuery deliveryQuery)
        {
            _deliveryQuery = deliveryQuery;
        }
        
        [HttpPost("api/driver/activeRouteTrip")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default) => 
            await _deliveryQuery.GetDeliveryIsActiveAsync(HttpContext.Items["UserId"]?.ToString());
    }
}