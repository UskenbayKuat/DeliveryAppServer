using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.HubInterfaces
{
    public interface IChatHub
    {
        public Task ConnectedAsync(string userId, string connectId);
        public Task DisconnectedAsync(string connectId);
    }
}