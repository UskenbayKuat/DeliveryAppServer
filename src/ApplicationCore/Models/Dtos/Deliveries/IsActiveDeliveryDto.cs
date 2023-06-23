using System;
using System.Collections.Generic;
using ApplicationCore.Models.Dtos.Orders;

namespace ApplicationCore.Models.Dtos.Deliveries
{
    public class IsActiveDeliveryDto
    {
        public string StartCityName { get; set; }
        public string FinishCityName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsStartVisible { get; set; }
        public int HandOverCount { get; set; }
        public int ActiveCount { get; set; }
        public int OrderCount { get; set; }
        public List<OrderDto> OrderInfos { get; set; }
    }
}