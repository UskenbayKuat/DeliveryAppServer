using ApplicationCore.Entities;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Models.Entities.Orders
{
    public class State : BaseEntity
    {
        public State(GeneralState stateValue, string name)
        {
            StateValue = stateValue;
            Name = name;
        }

        public GeneralState StateValue { get; private set; }
        public string Name { get; private set; }
    }
}