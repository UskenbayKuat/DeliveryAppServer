using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities
{
    public class ClientPackage : BaseEntity
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int StartCityId { get; set; }
        public City StartCity { get; set; }
        public int FinishCityId { get; set; }
        public City FinishCity { get; set; }
        public Package Package { get; set; }
        public int PackageId { get; set; }
        public DateTime DateTime { get; set; }
        public CarType CarType { get; set; }
        public int CarTypeId { get; set; }
        public bool IsSingle { get; set; }
        public decimal Price { get; set; }
        
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public string HubId { get; set; }
        
    }
}