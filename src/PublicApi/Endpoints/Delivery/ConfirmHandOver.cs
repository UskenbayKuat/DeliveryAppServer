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
        private readonly IOrderCommand _orderCommand;
        private readonly IMapper _mapper;

        public ConfirmHandOver(IOrderCommand orderCommand, IMapper mapper)
        {
            _orderCommand = orderCommand;
            _mapper = mapper;
        }

        [HttpPost("api/driver/confirmHandOver")]
        public override async Task<ActionResult> HandleAsync([FromBody] ConfirmHandOverCommand request,
            CancellationToken cancellationToken = default) =>
            await _orderCommand.ConfirmHandOverAsync(_mapper.Map<ConfirmHandOverInfo>(request), cancellationToken);
    }
}