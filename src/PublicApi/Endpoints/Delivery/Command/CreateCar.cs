using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Models.Dtos;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;
using PublicApi.Commands.Deliveries;

namespace PublicApi.Endpoints.Delivery.Command
{
    [Authorize]
    public class CreateCar: EndpointBaseAsync.WithRequest<CreateCarCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly IDriver _driver;

        public CreateCar(IMapper mapper, IDriver driver)
        {
            _mapper = mapper;
            _driver = driver;
        }

        [HttpPost("api/driver/createCar")]
        public override async Task<ActionResult> HandleAsync([FromBody]CreateCarCommand request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var userId = HttpContext.Items["UserId"]?.ToString();
                var createCarDto = _mapper.Map<CreateCarDto>(request).SetUserId(userId);
                await _driver.AddCarAsync(createCarDto);
                return new NoContentResult();
            }            
            catch(CarExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Ошибка");
            }
        }
    }
}