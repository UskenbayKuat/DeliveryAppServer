using System;

namespace ApplicationCore.Entities.AppEntities
{
    public class Order : BaseEntity
    {
        public Order ( DateTime orderDate, decimal orderCost)
        {
            OrderDate = orderDate;
            OrderCost = orderCost;
        }
        
        public RouteTrip RouteTrip { get; private set;}
        public Status Status { get; private set; }
        public DateTime OrderDate { get; private set;}
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; private set; }
        public DateTime? CancellationDate { get; private set; }
        public decimal OrderCost { get; private set; }
        public bool IsDeleted { get; set; }

        public void AddOrderDate(RouteTrip routeTrip, Status status)
        {
            RouteTrip = routeTrip;
            Status = status;
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