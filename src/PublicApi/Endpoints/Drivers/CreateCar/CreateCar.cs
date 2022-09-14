using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.CreateCar
{
    public class CreateCar: EndpointBaseAsync.WithRequest<CreateCarCommand>.WithActionResult<CreateCarResult>
    {
        private readonly IMapper _mapper;
        private readonly ICreateCar _createCar;

        public CreateCar(IMapper mapper, ICreateCar createCar)
        {
            _mapper = mapper;
            _createCar = createCar;
        }

        [HttpPost("/drivers/createCar")]
        public override async Task<ActionResult<CreateCarResult>> HandleAsync([FromBody]CreateCarCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await _createCar.CreateAuto(_mapper.Map<CreateCarInfo>(request), cancellationToken);
        }
    }
}