using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities.Routes
{
    public class RoutePrice : BaseEntity
    {
        public RoutePrice(double price)
        {
            Price = price;
        }
        public Route Route { get; set;}
        public double Price { get; private set; }
    }
}