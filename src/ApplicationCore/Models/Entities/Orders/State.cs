using ApplicationCore.Entities;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Models.Entities.Orders
{
    public class State : BaseEntity
    {
        public State(GeneralState stateValue, string name, string stateName)
        {
            StateValue = stateValue;
            Name = name;
            StateName = stateName;
        }

        public GeneralState StateValue { get; private set; }
        public string StateName { get; private set; }
        public string Name { get; private set; }
    }
}