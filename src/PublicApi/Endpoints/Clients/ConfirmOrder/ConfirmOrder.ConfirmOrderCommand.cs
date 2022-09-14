using System;
using ApplicationCore.Entities.AppEntities;

namespace PublicApi.Endpoints.Clients.ConfirmOrder
{
    public class ConfirmOrderCommand
    {
        public int ClientId { get; set; }
        public City StartCity { get; set; }
        public City FinishCity { get; set; }
        public Package Package { get; set; }
        public DateTime DateTime { get; set; }
        public CarType CarType { get; set; }
        public bool IsSingle { get; set; }
        public double Price { get; set; }
    }
}