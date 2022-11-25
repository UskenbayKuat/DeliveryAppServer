using System;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;

namespace ApplicationCore.Entities.Values
{
    public class ClientPackageInfo
    {
        public int ClientId { get; set; }
        public int StartCityId { get; set; }
        public int FinishCityId { get; set; }
        public Package Package { get; set; }
        public DateTime DateTime { get; set; }
        public int CarTypeId { get; set; }
        public bool IsSingle { get; set; }
        public decimal Price { get; set; }
    }
}