using System;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Models.Entities.Locations;

namespace ApplicationCore.Models.Entities.Orders
{
    public class Order : BaseEntity
    {
        [NotMapped]
        public const string StorageAddress = "Досклада";
        public Order(bool isSingle, double price, DateTime deliveryDate, string description, string addressTo, string addressFrom)
        {
            DeliveryDate = deliveryDate;
            IsSingle = isSingle;
            Price = price;
            Description = description;
            AddressTo = addressTo;
            AddressFrom = string.IsNullOrEmpty(addressFrom) ? StorageAddress : addressFrom;
        }
        public CarType CarType { get;  set;}
        public Client Client { get;  set;}
        public Package Package { get;  set;}
        public bool IsSingle { get; private set;}
        public DateTime DeliveryDate { get; private set; }
        public State State { get; set; }
        public double Price { get; private set;}
        public string SecretCode { get; private set;}
        public Location Location { get;  set;}
        public Route Route { get; set;}
        public Delivery Delivery { get;  set;}
        public string AddressTo { get; private set; }
        public string AddressFrom { get; private set; }
        public string Description { get; private set; }

        public Order SetSecretCode()
        {
            SecretCode = Guid.NewGuid().ToString("N")[..4].ToUpper();
            return this;
        }
        public Order SetSecretCodeEmpty()
        {
            SecretCode = string.Empty;
            return this;
        }

    }
}