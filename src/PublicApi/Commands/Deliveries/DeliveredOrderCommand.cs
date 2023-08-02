using MediatR;

namespace PublicApi.Commands.Deliveries
{
    public class DeliveredOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public string UserId { get; private set; }
        public DeliveredOrderCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}
