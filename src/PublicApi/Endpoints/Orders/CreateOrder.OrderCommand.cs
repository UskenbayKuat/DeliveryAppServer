using System;
using System.Collections.Generic;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Routes;

namespace PublicApi.Endpoints.Orders
{
    public class OrderCommand
    {
        public int ClientPackageId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateTime { get; set; }
        public string FullName { get; set; }
        public Route Route { get; set; }
        public bool IsSingle { get; set; }
        public decimal Price  { get; set; }
        public string Location  { get; set; }
        public Package Package { get;  set;}
    }
}