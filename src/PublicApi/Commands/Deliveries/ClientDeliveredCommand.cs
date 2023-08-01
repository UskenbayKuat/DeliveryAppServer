using MediatR;

namespace PublicApi.Commands.Deliveries
{
    public class ClientDeliveredCommand : IRequest
    {
        public int OrderId { get; set; }
        public string UserId { get; private set; }
        public ClientDeliveredCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}
