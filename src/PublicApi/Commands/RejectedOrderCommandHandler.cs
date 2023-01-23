using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using AutoMapper;
using MediatR;

namespace PublicApi.Commands
{
    public class RejectedOrderCommandHandler : IRequestHandler<RejectedOrderCommand, string>
    {
        private readonly IDriver _driverService;
        private readonly IChatHub _chatHub;
        private readonly IMapper _mapper;

        public RejectedOrderCommandHandler(IDriver driverService, IMapper mapper, IChatHub chatHub)
        {
            _driverService = driverService;
            _mapper = mapper;
            _chatHub = chatHub;
        }

        public async Task<string> Handle(RejectedOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _driverService.RejectNextFindDriverAsync(request.UserId, request.OrderId);
            var connectionId = await _chatHub.FindDriverConnectionIdAsync(order, cancellationToken);
            return connectionId;
        }
    }
}