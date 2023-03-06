using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Commands
{
    public class StartDeliveryCommand : IRequest<ActionResult>
    {
        public string UserId { get; private set; }

        public StartDeliveryCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}