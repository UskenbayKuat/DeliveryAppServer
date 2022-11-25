namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class RejectedClientPackage : BaseEntity
    {
        public RouteTrip RouteTrip { get; set; }
        public ClientPackage ClientPackage { get; set; }
    }
}