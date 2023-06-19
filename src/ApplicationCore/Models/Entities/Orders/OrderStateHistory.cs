using System;
using ApplicationCore.Models.Entities.Orders;

namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class OrderStateHistory
    {
        public Order Order { get; set; }
        public State State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}