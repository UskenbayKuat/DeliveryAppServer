using System;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Entities.Locations;
using MediatR;

namespace PublicApi.Commands.Orders
{
    public class CreateOrderCommand : IRequest
    {
        public string StartCityName { get; set; }
        public string FinishCityName { get; set; }
        public Package Package { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string CarTypeName { get; set; }
        public bool IsSingle { get; set; }
        public double Price { get; set; }
        public Location Location { get; set; }
        public string AddressTo { get; set; }
        public string Description { get; set; }
        public string AddressFrom { get; set; } = string.Empty;
        public string UserId { get; private set; }
        public CreateOrderCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}