namespace ApplicationCore.Entities.AppEntities.Orders
{
    public class Status : BaseEntity
    {
        public Status(int id, string state)
        {
            Id = id;
            State = state;
        }

        public string State { get; private set; }

        public void UpdateState(string state)
        {
            State = state;
        }
    }
}