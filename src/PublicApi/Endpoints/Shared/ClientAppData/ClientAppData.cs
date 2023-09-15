using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos.Shared;
using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.ClientAppData
{
    [Authorize]
    public class ClientAppData : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryAppData<ClientAppDataDto> _deliveryAppData;

        public ClientAppData(IDeliveryAppData<ClientAppDataDto> deliveryAppData)
        {
            _deliveryAppData = deliveryAppData;
        }

        [HttpPost("api/client/appData")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new CancellationToken()) 
            => await _deliveryAppData.SendDataAsync(cancellationToken);
    }
}