using ApplicationCore.Enums;

namespace ApplicationCore.Entities.AppEntities
{
    public class Status : BaseEntity
    {
        public State State { get; set; }
    }
}