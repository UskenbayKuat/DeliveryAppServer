using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class RouteTrip : BaseEntity
    {
        public RouteTrip()
        {
            IsActive = true;
        }

        public Driver Driver { get; set;}
        public RouteDate RouteDate { get; set; }
        public bool IsActive { get; private set; }

        public void ChangeStatusToNotActive()
        {
            IsActive = false;
        }
    }
}