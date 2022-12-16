using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Clients.Order
{
    public class CalculateOrder : EndpointBaseAsync.WithRequest<OrderCommand>.WithActionResult
    {
        private readonly ICalculate _calculate;
        private readonly IMapper _mapper;


        public CalculateOrder(ICalculate calculate, IMapper mapper)
        {
            _calculate = calculate;
            _mapper = mapper;
        }

        [HttpPost("api/client/calculate")]
        public override async Task<ActionResult> HandleAsync([FromBody]OrderCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _calculate.CalculateAsync(_mapper.Map<OrderInfo>(request), cancellationToken);
        }
    }
}