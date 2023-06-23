namespace ApplicationCore.Models.Values
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