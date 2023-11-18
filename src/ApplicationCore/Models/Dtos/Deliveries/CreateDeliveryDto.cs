using System;
using ApplicationCore.Models.Entities.Locations;

namespace ApplicationCore.Models.Dtos.Deliveries
{
    public class CreateDeliveryDto
    {
        public Guid StartCityId { get; set; }
        public Guid FinishCityId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Location Location { get; set; }
        public Guid UserId { get; set; }
    }
}