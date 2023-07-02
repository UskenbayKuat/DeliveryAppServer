using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities.Orders;

namespace ApplicationCore.Models.Entities.Orders
{
    public class DeliveryStateHistory : BaseEntity
    {
        public Delivery Delivery { get; set; }
        public State State { get; set; }
    }
}