namespace ApplicationCore.Entities.AppEntities.UIMessages
{
    public class MessageForUser : BaseEntity
    {
        public MessageForUser(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}