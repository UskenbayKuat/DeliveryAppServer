using System;

namespace ApplicationCore.Entities.AppEntities
{
    public class DriverKit : BaseEntity
    {
        public Driver Driver { get; set; }
        public Kit Kit { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}