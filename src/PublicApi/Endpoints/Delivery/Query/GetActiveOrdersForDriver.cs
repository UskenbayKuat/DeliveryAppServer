using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery
{
    [Authorize]
    public class GetActiveOrdersForDriver : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryQuery _deliveryQuery;

        public GetActiveOrdersForDriver(IDeliveryQuery deliveryQuery)
        {
            _deliveryQuery = deliveryQuery;
        }
        
        [HttpPost("api/driver/activeOrders")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default) => 
            await _deliveryQuery.GetActiveOrdersForDriverAsync(HttpContext.Items["UserId"]?.ToString());
    }
}