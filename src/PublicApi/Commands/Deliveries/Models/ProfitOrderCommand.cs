using MediatR;

namespace PublicApi.Commands.Deliveries.Models
{
    public class ProfitOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public string UserId { get; private set; }
        public ProfitOrderCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}
