using MediatR;

namespace PublicApi.Commands.Deliveries
{
    public class RejectedOrderCommand : IRequest    
    {
        public int OrderId { get; set; }
    }
}