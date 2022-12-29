using System;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;

namespace PublicApi.Endpoints.Delivery
{
    public class DeliveryCommand
    {
        public int OrderId { get; set; }
        public City StartCity { get; set; }
        public City FinishCity { get; set; }
        public Package Package { get;  set;}
        public bool IsSingle { get; set; }
        public decimal Price  { get; set; }
        public string ClientPhoneNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string Location  { get; set; }
        
    }
}