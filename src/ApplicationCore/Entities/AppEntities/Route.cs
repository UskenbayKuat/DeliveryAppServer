using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApplicationCore.Entities.AppEntities
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