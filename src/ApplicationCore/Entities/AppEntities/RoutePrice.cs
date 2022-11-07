using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities
{
    public class RoutePrice : BaseEntity
    {
        [ForeignKey("Route")]
        public int RouteId { get; set; }
        public Route Route { get; set; }
        public decimal Price { get; set; }
    }
}