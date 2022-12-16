using System;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values.Enums;

namespace ApplicationCore.Entities.Values
{
    public class DeliveryInfo
    {
        public string DeliveryState { get; set; }
        public string DriverName { get; set; }
        public string DriverSurname { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string CarNumber { get; set; }
        public int ClientPackageId { get; set; }
        public string ClientPhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; } 
        public City StartCity { get; set; }
        public City FinishCity { get; set; }
        public bool IsSingle { get; set; }
        public decimal Price  { get; set; }
        public string Location  { get; set; }
        public string StateName  { get; set; }
        public Package Package { get;  set;}

        public DeliveryInfo SetClientData(string name, string surname, string phoneNumber = "")
        {
            ClientName = name;
            ClientSurname = surname;
            ClientPhoneNumber = phoneNumber;
            return this;
        }
        
        public DeliveryInfo SetDriverData(string name, string surname, string phoneNumber = "")
        {
            DriverName = name;
            DriverSurname = surname;
            DriverPhoneNumber = phoneNumber;
            return this;
        }
    }
}