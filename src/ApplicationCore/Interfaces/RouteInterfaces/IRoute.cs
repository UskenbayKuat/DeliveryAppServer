using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Interfaces.RouteInterfaces
{
    public interface IRoute
    {
        Task<Route> GetByCitiesIdAsync(int startCityId, int finishCityId);
        Task<Route> GetByCitiesNameAsync(string startCityName, string finishCityName);
    }
}