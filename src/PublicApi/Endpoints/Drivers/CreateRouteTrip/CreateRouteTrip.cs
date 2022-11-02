using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Services;

namespace PublicApi.Endpoints.Drivers.CreateRouteTrip
{
    public class CreateRouteTrip : EndpointBaseAsync.WithRequest<CreateRouteTripCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly IRouteTrip _routeTrip;
        private readonly IValidation _validation;
        private readonly UserService _userService;

        public CreateRouteTrip(IMapper mapper, IValidation validation,IRouteTrip routeTrip, UserService userService)
        {
            _mapper = mapper;
            _validation = validation;
            _routeTrip = routeTrip;
            _userService = userService;
        }
        
        [HttpPost("api/drivers/RouteTrip")]
        public override async Task<ActionResult> HandleAsync([FromBody]CreateRouteTripCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            //Exception for userService has method First
            try
            {
                if (!_validation.ValidationDate(request.TripTime))
                {
                    return BadRequest();
                }
                return await _routeTrip.CreateRouteTrip(_mapper.Map<RouteInfo>(request), _userService.GetUserId(HttpContext), cancellationToken);
            }
            catch (NotExistUserException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch
            {
                return new BadRequestObjectResult("User not found");
            }
        }

    }
}