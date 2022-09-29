using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.CarBrands
{
    public class GetCarBrands : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly ICarBrand _carBrand;
        public GetCarBrands(ICarBrand carBrand)
        {
            _carBrand = carBrand;
        }
        [HttpGet("api/CarBrands")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new()) 
            => _carBrand.SendCarBrands(cancellationToken);
    }
}