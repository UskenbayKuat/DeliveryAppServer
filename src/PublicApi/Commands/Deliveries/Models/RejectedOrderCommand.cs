using MediatR;
using System;

namespace PublicApi.Commands.Deliveries.Models
{
    public class RejectedOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }
    }
}