using MediatR;
using System;

namespace PublicApi.Commands.Deliveries.Models
{
    public class ProfitOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; private set; }
        public ProfitOrderCommand SetUserId(string userId)
        {
            UserId = Guid.Parse(userId);
            return this;
        }
    }
}
