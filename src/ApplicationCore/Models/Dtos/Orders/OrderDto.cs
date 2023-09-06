using System;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Locations;

namespace ApplicationCore.Models.Dtos.Orders
{
    public class OrderDto : BaseOrderDto
    {
        public string CarTypeName { get; set; }
        public string StateName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsProfit { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsPendingForHandOver { get; set; }
    }
}