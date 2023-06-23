using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Commands
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