using ApplicationCore.Enums;

namespace ApplicationCore.Entities.AppEntities
{
    public class Status : BaseEntity
    {
        public Status(State state)
        {
            State = state;
        }

        public State State { get; private set; }

        public void UpdateState(State state)
        {
            State = state;
        }
    }
}