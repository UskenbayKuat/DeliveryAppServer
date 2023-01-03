using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using AutoMapper;
using MediatR;

namespace PublicApi.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
    {
        private readonly IOrder _order;
        private readonly IMapper _mapper;
        private readonly IChatHub _chatHub;

        public CreateOrderCommandHandler(IOrder order, IMapper mapper, IChatHub chatHub)
        {
            _order = order;
            _mapper = mapper;
            _chatHub = chatHub;
        }

        public async Task<string> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _order.CreateAsync(_mapper.Map<OrderInfo>(request), request.UserId, cancellationToken);
            var connectionId = await _chatHub.FindDriverConnectionIdAsync(order, cancellationToken);
            return connectionId;
        }
    }
}