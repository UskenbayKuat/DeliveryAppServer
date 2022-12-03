using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values.Enums;

namespace ApplicationCore.Entities.Values
{
    public class OrderInfo : ClientPackageInfoToDriver
    {
        public string OrderState { get; set; }
        public string DriverName { get; set; }
        public string DriverSurname { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string CarNumber { get; set; }
        
        public OrderInfo SetDriverData(string name, string surname, string phoneNumber = "")
        {
            DriverName = name;
            DriverSurname = surname;
            DriverPhoneNumber = phoneNumber;
            return this;
        }
    }
}