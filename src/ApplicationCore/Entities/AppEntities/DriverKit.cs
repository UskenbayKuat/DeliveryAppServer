using System;

namespace ApplicationCore.Entities.AppEntities
{
    public class DriverKit : BaseEntity
    {
        public Driver Driver { get; private set; }
        public Kit Kit { get; private set; }
        public DateTime PurchaseDate { get; private set; }

        public DriverKit( DateTime purchaseDate)
        {
            PurchaseDate = purchaseDate;
        }

        public void AddDriverAndKit(Driver driver, Kit kit)
        {
            Driver = driver;
            Kit = kit;
        }
    }
}