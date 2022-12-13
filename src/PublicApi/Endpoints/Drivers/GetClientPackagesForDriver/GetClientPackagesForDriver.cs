using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.GetClientPackagesForDriver
{
    [Authorize]
    public class GetClientPackagesForDriver : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDriver _driver;

        public GetClientPackagesForDriver(IDriver driver)
        {
            _driver = driver;
        }
        
        [HttpPost("api/drivers/ClientPackagesForDriver")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default) => 
            await _driver.SendOnReviewOrdersToDriverAsync(HttpContext.Items["UserId"]?.ToString());
    }
}