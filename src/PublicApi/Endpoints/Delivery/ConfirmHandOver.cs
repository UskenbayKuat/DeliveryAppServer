using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;

namespace PublicApi.Endpoints.Delivery
{
    public class ConfirmHandOver : EndpointBaseAsync.WithRequest<ConfirmHandOverCommand>.WithActionResult
    {
        private readonly IOrder _order;
        private readonly IMapper _mapper;

        public ConfirmHandOver(IOrder order, IMapper mapper)
        {
            _order = order;
            _mapper = mapper;
        }

        [HttpPost("api/driver/confirmHandOver")]
        public override async Task<ActionResult> HandleAsync([FromBody] ConfirmHandOverCommand request,
            CancellationToken cancellationToken = default) =>
            await _order.ConfirmHandOverAsync(_mapper.Map<ConfirmHandOverInfo>(request), cancellationToken);
    }
}