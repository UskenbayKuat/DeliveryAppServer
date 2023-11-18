using System;

namespace PublicApi.Commands.Orders.Models
{
    public class CancelOrderCommand
    {
        public Guid OrderId { get; set; }
    }
}