using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities.Routes
{
    public class Route : BaseEntity
    {
        [ForeignKey("StartCity")]
        public int StartCityId { get; set; }
        public City StartCity { get; set; }
        
        [ForeignKey("FinishCity")]
        public int FinishCityId { get; set; }
        public City FinishCity { get; set; }
    }
}