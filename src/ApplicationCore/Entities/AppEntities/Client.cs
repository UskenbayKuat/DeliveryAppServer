namespace ApplicationCore.Entities.AppEntities
{
    public class Client : BaseEntity
    {
        public string UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsValid { get; set; }
    }
}