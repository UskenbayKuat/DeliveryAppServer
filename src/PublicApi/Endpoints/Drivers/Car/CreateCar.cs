using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Models.Values;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.Car
{
    [Authorize]
    public class CreateCar: EndpointBaseAsync.WithRequest<CarCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly IDriver _driver;

        public CreateCar(IMapper mapper, IDriver driver)
        {
            _mapper = mapper;
            _driver = driver;
        }

        [HttpPost("api/driver/createCar")]
        public override async Task<ActionResult> HandleAsync([FromBody]CarCommand request,
            CancellationToken cancellationToken = new CancellationToken())
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
                return new BadRequestObjectResult(ex.Message);
            }
            catch
            {
                return new BadRequestObjectResult("Not correct data");
            }
        }
    }
}