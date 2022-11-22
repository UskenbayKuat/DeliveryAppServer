using ApplicationCore.Enums;

namespace ApplicationCore.Entities.AppEntities
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