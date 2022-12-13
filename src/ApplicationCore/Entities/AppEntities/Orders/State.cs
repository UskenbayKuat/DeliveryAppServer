namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class State : BaseEntity
    {
        public State(int id)
        {
            Id = id;
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}