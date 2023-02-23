using System;
using System.Collections.Generic;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class Delivery : BaseEntity
    {
        public Delivery (DateTime deliveryDate)
        {
            DeliveryDate = deliveryDate;
            CreatedAt = DateTime.Now;
        }
        
        public State State { get; set; }
        public Driver Driver { get; set;}
        public Route Route { get; set;}

        public DateTime CreatedAt { get; private set;}
        public DateTime DeliveryDate { get; set; }
        public DateTime? CompletionDate { get; private set; }
        public DateTime? CancellationDate { get; private set; }
        public bool IsDeleted { get; set; }
        public List<Order> Orders { get; private set; } = new();


        public void AddOrder(Order order)
        {
            Orders?.Add(order);
        }
        public void UpdateCompletionDate(DateTime dateTime)
        {
            CompletionDate = dateTime;
        }
        public void UpdateCancellationDate(DateTime dateTime)
        {
            CancellationDate = dateTime;
        }
    }
}