using System;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Models.Values;

namespace ApplicationCore.Entities.Values
{
    public class DeliveryInfo : OrderInfo
    {
        public string DeliveryState { get; set; }
        public string DriverName { get; set; }
        public string DriverSurname { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string CarNumber { get; set; }
    }
}