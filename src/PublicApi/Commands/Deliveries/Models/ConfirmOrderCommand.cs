using MediatR;
using System;

namespace PublicApi.Commands.Deliveries.Models
{
    public class ConfirmOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }
    }
}