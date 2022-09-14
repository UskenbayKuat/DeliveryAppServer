using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.GetKits
{
    public class GetKits : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IGetKits _getKits;

        public GetKits(IGetKits getKits)
        {
            _getKits = getKits;
        }
        
        [HttpGet("api/Kits")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new()) 
            => _getKits.SendKits(cancellationToken);
    }
}