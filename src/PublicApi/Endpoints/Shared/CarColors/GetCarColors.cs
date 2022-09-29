using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.CarColors
{
    public class GetCarColors : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly ICarColor _carColor;
        public GetCarColors(ICarColor carColor)
        {
            _carColor = carColor;
        }
        [HttpGet("api/CarColors")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new()) 
            => _carColor.SendCarColors(cancellationToken);
    }
}