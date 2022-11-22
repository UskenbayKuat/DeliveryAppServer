namespace ApplicationCore.Entities.AppEntities
{
    public class RejectOrder : BaseEntity
    {
        public RouteTrip RouteTrip { get; set; }
        public ClientPackage ClientPackage { get; set; }
    }
}