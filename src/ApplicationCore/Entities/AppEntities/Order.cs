using System;

namespace ApplicationCore.Entities.AppEntities
{
    public class Order : BaseEntity
    {
        public ClientPackage ClientPackage { get; set; }
        public RouteTrip RouteTrip { get; set; }
        public Status Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public decimal OrderCost { get; set; }
        public bool IsDeleted { get; set; }
    }
}