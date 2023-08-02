using ApplicationCore.Models.Dtos.Orders;
using System.Collections.Generic;
using System;

namespace ApplicationCore.Models.Dtos.Deliveries
{
    public class HistoryDeliveryDto
    {
        public string StartCityName { get; set; }
        public string FinishCityName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string StateName { get; set; }
        public List<OrderDto> OrderDtoList { get; set; }
    }
}
