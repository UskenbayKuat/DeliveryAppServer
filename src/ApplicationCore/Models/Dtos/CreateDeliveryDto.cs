using System;
using ApplicationCore.Entities.AppEntities.Locations;

namespace ApplicationCore.Models.Dtos
{
    public class CreateDeliveryDto
    {
        public int StartCityId { get; set; }
        public int FinishCityId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Location Location { get; set; }
        public string UserId { get; set; }
    }
}