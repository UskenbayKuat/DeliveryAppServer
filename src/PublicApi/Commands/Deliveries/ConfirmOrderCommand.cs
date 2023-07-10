using MediatR;

namespace PublicApi.Commands.Orders
{
    public class ConfirmOrderCommand : IRequest
    {
        public int OrderId { get; set; }
    }
}