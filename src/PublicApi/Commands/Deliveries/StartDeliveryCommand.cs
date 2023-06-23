using MediatR;

namespace PublicApi.Commands.Deliveries
{
    public class StartDeliveryCommand : IRequest
    {
        public StartDeliveryCommand(string userId)
        {
            UserId = userId;
        }
        public string UserId { get; private set; }
    }
}