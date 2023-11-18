using System;
using ApplicationCore.Models.Entities.Locations;
using MediatR;

namespace PublicApi.Commands.Deliveries.Models
{
    public class CreateDeliveryCommand : IRequest
    {
        public Guid StartCityId { get; set; }
        public Guid FinishCityId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Location Location { get; set; }
        public Guid UserId { get; private set; }
        public CreateDeliveryCommand SetUserId(string userId)
        {
            UserId = Guid.Parse(userId);
            return this;
        }
    }
}