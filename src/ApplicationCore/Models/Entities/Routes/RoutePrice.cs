using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities.Routes
{
    public class RoutePrice : BaseEntity
    {
        public RoutePrice(int id, int routeId, double price)
        {
            Id = id;
            RouteId = routeId;
            Price = price;
        }
        public int RouteId { get; private set; }

        public Route Route { get; set;}
        public double Price { get; private set; }
    }
}