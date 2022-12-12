using System;
using System.Collections.Generic;
using ApplicationCore.Entities.Values.Enums;

namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class Order : BaseEntity
    {
        public Order (DateTime orderDate)
        {
            OrderDate = orderDate;
            OrderState = OrderState.New;
        }
        
        public RouteTrip RouteTrip { get; set;}
        public OrderState OrderState { get; set; }
        public DateTime OrderDate { get; private set;}  
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; private set; }
        public DateTime? CancellationDate { get; private set; }
        public decimal OrderCost { get; private set; }
        public bool IsDeleted { get; set; }
        public List<ClientPackage> ClientPackages { get;  set; } = new();
        

        public void UpdateStatus(OrderState status)
        {
            OrderState = status;
        }

        public void UpdateCompletionDate(DateTime dateTime)
        {
            CompletionDate = dateTime;
        }
        public void UpdateCancellationDate(DateTime dateTime)
        {
            CancellationDate = dateTime;
        }
        public void UpdateOrderCost(decimal cost)
        {
            OrderCost = cost;
        }
    }
}