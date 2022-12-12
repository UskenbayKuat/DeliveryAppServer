using System;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values.Enums;

namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class ClientPackage : BaseEntity
    {
        public ClientPackage(bool isSingle, decimal price)
        {
            IsSingle = isSingle;
            CreatedAt = DateTime.Now;
            Price = price;
            ClientPackageState = ClientPackageState.New;
        }

        public CarType CarType { get;  set;}
        public Client Client { get;  set;}
        public Package Package { get;  set;}

        public bool IsSingle { get; private set;}
        public DateTime CreatedAt { get; private set; }
        public ClientPackageState ClientPackageState { get; private set; }
        public decimal Price { get; private set;}
        public Location Location { get;  set;}
        public Route Route { get; set;}

        public Order Order { get;  set;}
        
        public ClientPackage ChangeState(ClientPackageState state)
        {
            ClientPackageState = state;
            return this;
        }

    }
}