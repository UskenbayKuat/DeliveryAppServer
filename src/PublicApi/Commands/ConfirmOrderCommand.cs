using MediatR;

namespace PublicApi.Commands
{
    public class ConfirmOrderCommand : IRequest<string>
    {
        public int OrderId { get; set; }
    }
}