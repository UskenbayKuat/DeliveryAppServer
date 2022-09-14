using System;

namespace ApplicationCore.Entities.AppEntities
{
    public class DriverKit : BaseEntity
    {
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int KitId { get; set; }
        public Kit Kit { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}