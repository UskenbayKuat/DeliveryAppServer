using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.GetRouteTripForDriver
{
    [Authorize]
    public class GetRouteTripForDriver : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDriver _driver;

        public GetRouteTripForDriver(IDriver driver)
        {
            _driver = driver;
        }
        
        [HttpPost("api/driver/routeTripForDriver")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default) => 
            await _driver.SendRouteTripToDriverAsync(HttpContext.Items["UserId"]?.ToString());
    }
}