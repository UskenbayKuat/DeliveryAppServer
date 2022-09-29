using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.Kits
{
    public class GetKits : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IKit _kit;

        public GetKits(IKit kit)
        {
            _kit = kit;
        }
        
        [HttpGet("api/Kits")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new()) 
            => _kit.SendKits(cancellationToken);
    }
}