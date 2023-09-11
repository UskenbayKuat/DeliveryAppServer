using MediatR;

namespace PublicApi.Commands.Deliveries.Models
{
    public class ConfirmOrderCommand : IRequest
    {
        public int OrderId { get; set; }
    }
}