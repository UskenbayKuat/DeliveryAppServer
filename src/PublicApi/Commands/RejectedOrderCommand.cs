using System;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using MediatR;

namespace PublicApi.Commands
{
    public class RejectedOrderCommand : IRequest<string>
    {
        public int OrderId { get; set; }
        public string UserId { get; private set; }
        public RejectedOrderCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}