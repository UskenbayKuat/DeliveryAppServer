using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApplicationCore.Entities.AppEntities
{
    public class Route : BaseEntity
    {
        public int StartCityId { get; set; }
        public City StartCity { get; set; }
        public int FinishCityId { get; set; }
        public City FinishCity { get; set; }
        public decimal Price { get; set; }

    }
}