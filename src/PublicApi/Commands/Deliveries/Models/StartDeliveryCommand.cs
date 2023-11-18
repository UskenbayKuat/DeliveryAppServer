using MediatR;
using System;

namespace PublicApi.Commands.Deliveries.Models
{
    public class StartDeliveryCommand : IRequest
    {
        public StartDeliveryCommand(string userId)
        {
            UserId = Guid.Parse(userId);
        }
        public Guid UserId { get; private set; }
    }
}