namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class OnDriverReview : BaseEntity
    {
        public OnDriverReview()
        {
            OnReview = true;
        }
        public bool OnReview { get; set;}
        public RouteTrip RouteTrip { get; set;}
    }
}