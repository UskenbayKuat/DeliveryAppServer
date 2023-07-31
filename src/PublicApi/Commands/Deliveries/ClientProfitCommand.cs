using MediatR;

namespace PublicApi.Commands.Deliveries
{
    public class ClientProfitCommand : IRequest
    {
        public int OrderId{ get; set; }
        public string UserId { get; private set; }
        public ClientProfitCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}
