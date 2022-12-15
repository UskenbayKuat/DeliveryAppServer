using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.Delivery
{
    [Authorize]
    public class GetOnReviewOrdersForDriver : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDriver _driver;

        public GetOnReviewOrdersForDriver(IDriver driver)
        {
            _driver = driver;
        }
        
        [HttpPost("api/drivers/ClientPackagesForDriver")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default) => 
            await _driver.GetOnReviewOrdersForDriverAsync(HttpContext.Items["UserId"]?.ToString());
    }
}