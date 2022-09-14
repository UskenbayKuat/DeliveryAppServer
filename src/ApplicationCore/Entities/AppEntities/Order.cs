using System;

namespace ApplicationCore.Entities.AppEntities
{
    public class Order : BaseEntity
    {
        public int DriverKitId { get; set; }
        public DriverKit DriverKit { get; set; }
        public int ClientPackageId { get; set; }
        public ClientPackage ClientPackage { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? OrderStartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? DelayDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public decimal OrderCost { get; set; }
        public bool IsDeleted { get; set; }
    }
}