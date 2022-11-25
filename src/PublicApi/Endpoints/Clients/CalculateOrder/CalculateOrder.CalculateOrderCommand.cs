using System;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;

namespace PublicApi.Endpoints.Clients.CalculateOrder
{
    public class CalculateOrderCommand
    {
        public int StartCityId { get; set; }
        public int FinishCityId { get; set; }
        public Package Package { get; set; }
        public DateTime DateTime { get; set; }
        public int CarTypeId { get; set; }
        public bool IsSingle { get; set; }
        public double Price { get; set; }
    }
}