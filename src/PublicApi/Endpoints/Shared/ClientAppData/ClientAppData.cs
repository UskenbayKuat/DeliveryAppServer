using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces.SharedInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.ClientAppData
{
    public class ClientAppData : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IDeliveryAppData<ClientAppDataInfo> _deliveryAppData;

        public ClientAppData(IDeliveryAppData<ClientAppDataInfo> deliveryAppData)
        {
            _deliveryAppData = deliveryAppData;
        }

        [HttpPost("api/ClientAppData")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new CancellationToken()) 
            => _deliveryAppData.SendData(cancellationToken);
    }
}