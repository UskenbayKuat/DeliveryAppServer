using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Endpoints.Clients.CalculateOrder;

namespace PublicApi.Endpoints.Clients.ConfirmOrder
{
    public class ConfirmOrder : EndpointBaseAsync.WithRequest<ConfirmOrderCommand>.WithActionResult<ConfirmOrderResult>
    {
        private readonly IConfirmOrder _confirmOrder;
        private readonly IMapper _mapper;

        public ConfirmOrder(IConfirmOrder confirmOrder, IMapper mapper)
        {
            _confirmOrder = confirmOrder;
            _mapper = mapper;
        }

        [HttpPost("api/client/confirmOrder")]
        public override async Task<ActionResult<ConfirmOrderResult>> HandleAsync([FromBody]ConfirmOrderCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _confirmOrder.CreateClientPackage(_mapper.Map<ClientPackageInfo>(request), cancellationToken);
        }
    }
}