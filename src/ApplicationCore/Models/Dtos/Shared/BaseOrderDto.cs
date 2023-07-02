using System;
using ApplicationCore.Entities.AppEntities.Orders;

namespace ApplicationCore.Models.Dtos.Shared
{
    public class BaseOrderDto
    {
        public int OrderId { get; set; }
        public string StartCityName { get; set; }
        public string FinishCityName { get; set; }
        public Package Package { get; set; }
        public bool IsSingle { get; set; }
        public double Price { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; } 
        public string AddressTo { get; set; }
        public string AddressFrom { get; set; }
        public string Description { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}