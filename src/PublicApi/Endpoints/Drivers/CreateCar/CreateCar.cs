using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.CreateCar
{
    [Authorize]
    public class CreateCar: EndpointBaseAsync.WithRequest<CreateCarCommand>.WithActionResult<CreateCarResult>
    {
        private readonly IMapper _mapper;
        private readonly ICreateCar _createCar;

        public CreateCar(IMapper mapper, ICreateCar createCar)
        {
            _mapper = mapper;
            _createCar = createCar;
        }

        [HttpPost("api/drivers/createCar")]
        public override async Task<ActionResult<CreateCarResult>> HandleAsync([FromBody]CreateCarCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.First(c => c.Type == ClaimTypes.UserData).Value;
                return await _createCar.CreateAutoAsync(_mapper.Map<CreateCarInfo>(request), userId, cancellationToken);
            }
            catch
            {
                return new BadRequestObjectResult("Car not added");
            }
            
            
        }
    }
}