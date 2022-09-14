using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.GetCities
{
    public class GetCities : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IGetCities _getCities;

        public GetCities(IGetCities getCities)
        {
            _getCities = getCities;
        }

        [HttpGet("api/Cities")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new()) 
            => _getCities.SendCities(cancellationToken);
    }
}