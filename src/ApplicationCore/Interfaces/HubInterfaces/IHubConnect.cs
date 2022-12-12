using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.HubInterfaces
{
    public interface IHubConnect
    {
        public Task ConnectedUser(string userId, string connectId);
        public Task DisconnectedUser(string connectId);
    }
}