using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.GetCarTypes
{
    public class GetCarTypes : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IGetCarTypes _getCarTypes;

        public GetCarTypes(IGetCarTypes getCarTypes)
        {
            _getCarTypes = getCarTypes;
        }

        [HttpGet("api/CarTypes")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new()) 
            => _getCarTypes.SendCarTypes(cancellationToken);
    }
}