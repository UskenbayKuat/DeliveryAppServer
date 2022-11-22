using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;


namespace PublicApi.Endpoints.Drivers.CreateRouteTrip
{
    public class CreateRouteTrip : EndpointBaseAsync.WithRequest<CreateRouteTripCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly IRouteTrip _routeTrip;
        private readonly IValidation _validation;

        public CreateRouteTrip(IMapper mapper, IValidation validation, IRouteTrip routeTrip)
        {
            _mapper = mapper;
            _validation = validation;
            _routeTrip = routeTrip;
        }

        [HttpPost("api/drivers/RouteTrip")]
        public override async Task<ActionResult> HandleAsync([FromBody] CreateRouteTripCommand request,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (!_validation.ValidationDate(request.TripTime))
            {
                return BadRequest();
            }
            return await _routeTrip.CreateRouteTrip(_mapper.Map<RouteInfo>(request),
                HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }
    }
}
