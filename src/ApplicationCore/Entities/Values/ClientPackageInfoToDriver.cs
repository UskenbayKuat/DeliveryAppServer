using System;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.Values
{
    public class ClientPackageInfoToDriver
    {
        public int ClientPackageId { get; set; }
        public string ClientPhoneNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; } 
        public Route Route { get; set; }
        public bool IsSingle { get; set; }
        public decimal Price  { get; set; }
        public string Location  { get; set; }
        public Package Package { get;  set;}

        public ClientPackageInfoToDriver SetClientData(string name, string surname, string phoneNumber = "")
        {
            ClientName = name;
            ClientSurname = surname;
            ClientPhoneNumber = phoneNumber;
            return this;
        }

    }
}