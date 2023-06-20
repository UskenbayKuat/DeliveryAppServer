using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Delivery
{
    [Authorize]
    public class GetOnReviewOrdersForDriver : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryQuery _deliveryQuery;

        public GetOnReviewOrdersForDriver(IDeliveryQuery deliveryQuery)
        {
            _deliveryQuery = deliveryQuery;
        }
        
        [HttpPost("api/driver/onReviewOrders")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default) => 
            await _deliveryQuery.GetOnReviewOrdersForDriverAsync(HttpContext.Items["UserId"]?.ToString());
    }
}