#nullable enable
using System.Collections.Generic;
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
        private readonly IHubContext<Notification> _hubContext;

        public CreateRouteTrip(IMapper mapper, IRouteTrip routeTrip, IHubContext<Notification> hubContext)
        {
            _mapper = mapper;
            _routeTrip = routeTrip;
            _hubContext = hubContext;
        }

        [HttpPost("api/driver/createRouteTrip")]
        public override async Task<ActionResult> HandleAsync([FromBody] RouteTripCommand request,
            CancellationToken cancellationToken = new CancellationToken()) =>
            await _routeTrip.CreateAsync(_mapper.Map<RouteTripInfo>(request),
        HttpContext.Items["UserId"]?.ToString(), SendInfoForDriverAsync);


        private async Task SendInfoForDriverAsync(string connectionId, bool isEmpty)
        {
            if (isEmpty)
                await _hubContext.Clients.User(connectionId)
                    .SendCoreAsync("SendClientInfoToDriver", new[] { "Новый заказ" });
        }
        
        
            // if (!_validation.ValidationDate(request.TripTime))
            // {
            //     return BadRequest();
            // }
           
            
    }
}
