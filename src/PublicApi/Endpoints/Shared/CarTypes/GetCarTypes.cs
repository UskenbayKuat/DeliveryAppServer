using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.CarTypes
{
    public class GetCarTypes : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly ICarType _carType;
        public GetCarTypes(ICarType carType)
        {
            _carType = carType;
        }
        [HttpGet("api/CarTypes")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new()) 
            => _carType.SendCarTypes(cancellationToken);
    }
}