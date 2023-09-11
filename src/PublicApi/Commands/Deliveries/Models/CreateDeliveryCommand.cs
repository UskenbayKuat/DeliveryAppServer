using System;
using ApplicationCore.Models.Entities.Locations;
using MediatR;

namespace PublicApi.Commands.Deliveries.Models
{
    public class CreateDeliveryCommand : IRequest
    {
        public int StartCityId { get; set; }
        public int FinishCityId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Location Location { get; set; }
        public string UserId { get; private set; }
        public CreateDeliveryCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}