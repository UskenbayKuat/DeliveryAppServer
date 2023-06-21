using System;
using System.Collections.Generic;
using ApplicationCore.Entities.Values;
using ApplicationCore.Models.Values;

namespace ApplicationCore.Models.Dtos
{
    public class DeliveryDto
    {
        public string StartCityName { get; set; }
        public string FinishCityName { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool IsStartVisible { get; set; }
        public int HandOverCount { get; set; }
        public int ActiveCount { get; set; }
        public int OrderCount { get; set; }
        public List<OrderInfo> OrderInfos { get; set; }
    }
}