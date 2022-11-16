namespace ApplicationCore.Entities.AppEntities
{
    public class ChatHub : BaseEntity
    {
        public string UserId { get; private set;}
        public string ConnectionId { get; private set; }

        public ChatHub(string userId, string connectionId)
        {
            UserId = userId;
            ConnectionId = connectionId;
        }

        public void UpdateConnectId(string connectionId)
        {
            ConnectionId = connectionId;
        }
        public void RemoveConnectId()
        {
            ConnectionId = string.Empty;
        }
    }
}