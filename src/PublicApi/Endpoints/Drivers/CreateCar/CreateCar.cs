using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
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
        private readonly ICreateCar _createCar;

        public CreateCar(IMapper mapper, ICreateCar createCar)
        {
            _mapper = mapper;
            _createCar = createCar;
        }

        [HttpPost("api/drivers/createCar")]
        public override async Task<ActionResult> HandleAsync([FromBody]CreateCarCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
                return await _createCar.CreateAutoAsync(_mapper.Map<CreateCarInfo>(request),
                    (string)HttpContext.Items["UserId"], cancellationToken);
        }
    }
}