using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Values;
using Ardalis.ApiEndpoints;
using Infrastructure.Config.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.DriverAppData
{
    [Authorize]
    public class DriverAppData : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryAppData<DriverAppDataDto> _deliveryAppData;
        
        public DriverAppData(IDeliveryAppData<DriverAppDataDto> deliveryAppData)
        {
            _deliveryAppData = deliveryAppData;
        }

        [HttpPost("api/driver/appData")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new CancellationToken()) 
            => await _deliveryAppData.SendDataAsync(cancellationToken);
    }
}