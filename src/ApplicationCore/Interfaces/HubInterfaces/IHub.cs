using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.HubInterfaces
{
    public interface IHub
    {
        public Task SendClientInfoToDriver(string message);
    }
}