using ApplicationCore.Models.Dtos.Orders;

namespace ApplicationCore.Models.Dtos.Deliveries
{
    public class DeliveryDto : OrderDto
    {
        public string DeliveryState { get; set; }
        public string DriverName { get; set; }
        public string DriverSurname { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string CarNumber { get; set; }
    }
}