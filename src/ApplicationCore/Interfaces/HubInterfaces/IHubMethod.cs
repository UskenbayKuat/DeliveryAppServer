using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;

namespace ApplicationCore.Interfaces.HubInterfaces
{
    public interface IHubMethod
    {
        public Task SendClientInfoToDriver(List<OrderInfo> orderInfos);
        public Task SendDriverInfoToClient(string message);
    }
}