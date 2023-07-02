using System;
using ApplicationCore.Models.Dtos.Shared;

namespace ApplicationCore.Models.Dtos.Deliveries
{
    public class DeliveryDto : BaseOrderDto
    {
        public string DeliveryState { get; set; }
        public string DriverName { get; set; }
        public string DriverSurname { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string CarNumber { get; set; }
        public string SecretCode { get; set; }

    }
}