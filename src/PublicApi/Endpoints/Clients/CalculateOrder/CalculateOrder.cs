using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Endpoints.Drivers.CreateRouteTrip;

namespace PublicApi.Endpoints.Clients.CalculateOrder
{
    public class CalculateOrder : EndpointBaseAsync.WithRequest<CalculateOrderCommand>.WithActionResult<CalculateOrderResult>
    {
        private readonly ICalculate _calculate;
        private readonly IMapper _mapper;


        public CalculateOrder(ICalculate calculate, IMapper mapper)
        {
            _calculate = calculate;
            _mapper = mapper;
        }

        [HttpPost("api/client/calculate")]
        public override async Task<ActionResult<CalculateOrderResult>> HandleAsync([FromBody]CalculateOrderCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _calculate.Calculate(_mapper.Map<ClientPackageInfo>(request), cancellationToken);
        }
    }
}