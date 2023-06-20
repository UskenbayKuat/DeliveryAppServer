using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using System;
using ApplicationCore.Models.Entities.Locations;

namespace ApplicationCore.Models.Dtos
{
    public class CreateOrderDto
    {
        public string StartCityName { get; set; }
        public string FinishCityName { get; set; }
        public Package Package { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string CarTypeName { get; set; }
        public bool IsSingle { get; set; }
        public double Price { get; set; }
        public Location Location { get; set; }
        public string AddressTo { get; set; }
        public string AddressFrom { get; set; } = string.Empty;
        public string Description { get; set; }
        public string UserId { get; set; }

    }
}
