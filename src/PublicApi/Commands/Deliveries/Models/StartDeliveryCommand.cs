using MediatR;

namespace PublicApi.Commands.Deliveries.Models
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