using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using MediatR;

namespace PublicApi.Commands
{
    public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, string>
    {
        private readonly IDelivery _delivery;
        private readonly IContext _context;

        public ConfirmOrderCommandHandler(IDelivery delivery, IContext context)
        {
            _delivery = delivery;
            _context = context;
        }

        public async Task<string> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order  = await _delivery.AddToDeliveryAsync(request.OrderId);
            var chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == order.Client.UserId);
            return chatHub?.ConnectionId;
        }
    }
}