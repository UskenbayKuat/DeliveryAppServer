namespace ApplicationCore.Entities.AppEntities
{
    public class ChatHub : BaseEntity
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }

        public ChatHub(string userId, string connectionId)
        {
            UserId = userId;
            ConnectionId = connectionId;
        }
    }
}