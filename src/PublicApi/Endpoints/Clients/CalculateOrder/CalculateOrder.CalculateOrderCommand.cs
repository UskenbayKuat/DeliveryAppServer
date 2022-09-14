using System;
using ApplicationCore.Entities.AppEntities;

namespace PublicApi.Endpoints.Clients.CalculateOrder
{
    public class CalculateOrderCommand
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