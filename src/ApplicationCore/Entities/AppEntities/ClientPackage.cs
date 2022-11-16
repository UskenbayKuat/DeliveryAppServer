using System;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.AppEntities
{
    public class ClientPackage : BaseEntity
    {
        public ClientPackage(bool isSingle, decimal price)
        {
            IsSingle = isSingle;
            Price = price;
        }

        public CarType CarType { get; private set;}
        public Client Client { get; private set;}
        public Package Package { get; private set;}

        public bool IsSingle { get; private set;}
        public decimal Price { get; private set;}
        public Location Location { get; private set;}
        public RouteDate RouteDate { get; private set;}
        public Order Order { get; private set; }

        public void AddOrder(Order order)
        {
            Order = order;
        }

        public ClientPackage AddClientPackageData(CarType carType, Client client, Package package, Location location, RouteDate routeDate)
        {
            CarType = carType;
            Client = client;
            Package = package;
            Location = location;
            RouteDate = routeDate;
            return this;
        }
    }
}