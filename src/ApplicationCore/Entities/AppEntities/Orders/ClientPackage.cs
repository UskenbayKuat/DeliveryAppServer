using System;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class ClientPackage : BaseEntity
    {
        public ClientPackage(bool isSingle, decimal price)
        {
            IsSingle = isSingle;
            CreatedAt = DateTime.Now;
            Price = price;
        }

        public CarType CarType { get;  set;}
        public Client Client { get;  set;}
        public Package Package { get;  set;}

        public bool IsSingle { get; private set;}
        public DateTime CreatedAt { get; private set; }

        public decimal Price { get; private set;}
        public Location Location { get;  set;}
        public Route Route { get; set;}

        public Order Order { get;  set;}
        public OnDriverReview OnDriverReview { get; set; }

        public ClientPackage SetOnReview(bool onReview)
        {
            OnDriverReview.OnReview = onReview;
            return this;
        }
    }
}