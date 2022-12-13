using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.CreateCar
{
    [Authorize]
    public class CreateCar: EndpointBaseAsync.WithRequest<CreateCarCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly ICar _car;

        public CreateCar(IMapper mapper, ICar car)
        {
            _mapper = mapper;
            _car = car;
        }

        [HttpPost("api/drivers/createCar")]
        public override async Task<ActionResult> HandleAsync([FromBody]CreateCarCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
                return await _car.CreateAsync(_mapper.Map<CarInfo>(request),
                    HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }
}