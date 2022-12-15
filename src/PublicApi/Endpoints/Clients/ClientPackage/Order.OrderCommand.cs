using System;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;

namespace PublicApi.Endpoints.Clients.ClientPackage
{
    public class OrderCommand
    {
        public City StartCity { get; set; }
        public City FinishCity { get; set; }
        public Package Package { get; set; }
        public DateTime DateTime { get; set; }
        public CarType CarType { get; set; }
        public bool IsSingle { get; set; }
        public double Price { get; set; }
    }
}