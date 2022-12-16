using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DriverInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PublicApi.HubNotification;

namespace PublicApi.Endpoints.Delivery
{
    public class RejectOrder : EndpointBaseAsync.WithRequest<DeliveryCommand>.WithActionResult
    {
        private readonly IDriver _driverService;
        private readonly IHubContext<Notification> _hubContext;
        private readonly IMapper _mapper;

        public RejectOrder(IDriver driverService, IHubContext<Notification> hubContext, IMapper mapper)
        {
            _driverService = driverService;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpPost("api/rejectOrder")]
        public override async Task<ActionResult> HandleAsync(DeliveryCommand request,
            CancellationToken cancellationToken = default) =>
            await _driverService.RejectNextFindDriverAsync(
                driverUserId: HttpContext.Items["UserId"]?.ToString(),
                orderInfo: _mapper.Map<OrderInfo>(request),
                func: SendInfoForDriverAsync);
        
        private async Task SendInfoForDriverAsync(string driverConnectionId, OrderInfo info) =>
            await _hubContext.Clients.User(driverConnectionId).SendCoreAsync("SendClientInfoToDriver", new[] { new List<OrderInfo> { info } });
    }
}