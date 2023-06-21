using System;
using ApplicationCore.Entities;

namespace ApplicationCore.Models.Entities.Orders
{
    public class OrderStateHistory : BaseEntity
    {
        public Order Order { get; set; }
        public State State { get; set; }
    }
}