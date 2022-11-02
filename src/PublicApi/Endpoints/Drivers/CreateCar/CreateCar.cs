using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Services;

namespace PublicApi.Endpoints.Drivers.CreateCar
{
    [Authorize]
    public class CreateCar: EndpointBaseAsync.WithRequest<CreateCarCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly ICreateCar _createCar;
        private readonly UserService _userService;

        public CreateCar(IMapper mapper, ICreateCar createCar, UserService userService)
        {
            _mapper = mapper;
            _createCar = createCar;
            _userService = userService;
        }

        [HttpPost("api/drivers/createCar")]
        public override async Task<ActionResult> HandleAsync([FromBody]CreateCarCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await _createCar.CreateAutoAsync(_mapper.Map<CreateCarInfo>(request),
                    _userService.GetUserId(HttpContext), cancellationToken);
            }
            catch(NotExistUserException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch
            {
                return new BadRequestObjectResult("Car not added");
            }
        }
    }
}