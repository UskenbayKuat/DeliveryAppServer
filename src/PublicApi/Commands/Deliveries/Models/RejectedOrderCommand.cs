using MediatR;

namespace PublicApi.Commands.Deliveries.Models
{
    public class RejectedOrderCommand : IRequest
    {
        public int OrderId { get; set; }
    }
}