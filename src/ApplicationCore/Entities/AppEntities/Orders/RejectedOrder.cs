namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class RejectedOrder : BaseEntity
    {
        public Delivery Delivery { get; set; }
        public Order Order { get; set; }
    }
}