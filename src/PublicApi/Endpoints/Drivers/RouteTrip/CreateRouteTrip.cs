#nullable enable
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicApi.HubNotification;

namespace PublicApi.Endpoints.Drivers.RouteTrip
{
    [Authorize]
    public class CreateRouteTrip : EndpointBaseAsync.WithRequest<RouteTripCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly IRouteTrip _routeTrip;
        private readonly IValidation _validation;
        private readonly IHubContext<Notification> _hubContext;
        private readonly IDriver _driver;

        public CreateRouteTrip(IMapper mapper, IValidation validation, IRouteTrip routeTrip, IHubContext<Notification> hubContext, IDriver driver)
        {
            _mapper = mapper;
            _validation = validation;
            _routeTrip = routeTrip;
            _hubContext = hubContext;
            _driver = driver;
        }

        [HttpPost("api/driver/createRouteTrip")]
        public override async Task<ActionResult> HandleAsync([FromBody] RouteTripCommand request,
            CancellationToken cancellationToken = new CancellationToken()) =>
            await _routeTrip.CreateAsync(_mapper.Map<RouteTripInfo>(request),
        HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        
            // if (!_validation.ValidationDate(request.TripTime))
            // {
            //     return BadRequest();
            // }
           
            
    }
}
