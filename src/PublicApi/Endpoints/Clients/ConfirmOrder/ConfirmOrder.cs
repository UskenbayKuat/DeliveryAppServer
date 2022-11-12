using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Endpoints.Clients.CalculateOrder;

namespace PublicApi.Endpoints.Clients.ConfirmOrder
{
    public class ConfirmOrder : EndpointBaseAsync.WithRequest<ConfirmOrderCommand>.WithActionResult
    {
        private readonly IClientPackage _clientPackage;
        private readonly IMapper _mapper;

        public ConfirmOrder(IClientPackage clientPackage, IMapper mapper)
        {
            _clientPackage = clientPackage;
            _mapper = mapper;
        }

        [HttpPost("api/client/confirmOrder")]
        public override async Task<ActionResult> HandleAsync([FromBody]ConfirmOrderCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _clientPackage.CreateClientPackage(_mapper.Map<ClientPackageInfo>(request), (string)HttpContext.Items["UserId"], cancellationToken);
        }
    }
}