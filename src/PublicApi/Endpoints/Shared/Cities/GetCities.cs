using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.Cities
{
    public class GetCities : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly ICity _city;

        public GetCities(ICity city)
        {
            _city = city;
        }

        [HttpGet("api/Cities")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new()) 
            => _city.SendCities(cancellationToken);
    }
}