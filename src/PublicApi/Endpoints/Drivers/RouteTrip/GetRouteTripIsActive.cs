using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.RouteTrip
{
    [Authorize]
    public class GetRouteTripIsActive : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDriver _driver;

        public GetRouteTripIsActive(IDriver driver)
        {
            _driver = driver;
        }
        
        [HttpPost("api/driver/routeTripForDriver")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default) => 
            await _driver.GetRouteTripIsActiveAsync(HttpContext.Items["UserId"]?.ToString());
    }
}