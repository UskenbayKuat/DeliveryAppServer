using System;
using System.Collections.Generic;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Models.Entities.Locations;
using ApplicationCore.Models.Entities.Orders;

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
        public Location Location { get; set; }
        public bool IsDeleted { get; private set; }
        public List<Order> Orders { get; private set; } = new();


        public void AddOrder(Order order)
        {
            Orders?.Add(order);
        }
        public Delivery SetCompletionDate(DateTime dateTime)
        {
            CompletionDate = dateTime;
            return this;
        }
        public Delivery SetCancellationDate()
        {
            CancellationDate = DateTime.Now;
            IsDeleted = true;
            ModifiedDate = DateTime.Now;
            return this;
        }
        public Delivery SetDelete()
        {
            IsDeleted = true;
            ModifiedDate = DateTime.Now;
            return this;
        }
    }
}