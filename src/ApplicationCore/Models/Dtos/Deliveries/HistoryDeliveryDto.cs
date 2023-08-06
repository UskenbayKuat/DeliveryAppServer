using ApplicationCore.Models.Dtos.Orders;
using System.Collections.Generic;
using System;
using System.Linq;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Models.Dtos.Deliveries
{
    public class HistoryDeliveryDto
    {
        public int Id { get; set; }
        public string StartCityName { get; set; }
        public string FinishCityName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string StateName { get; set; }
        public GeneralState StateValue { get; set; }
        public int Hour => (FinishDate - StartDate).Hours;
        public double Price => OrderDtoList.Sum(x => x.Price);
        public List<OrderDto> OrderDtoList { get; set; } = new();
    }
}
