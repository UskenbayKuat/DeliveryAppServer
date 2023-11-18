using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Routes;

namespace ApplicationCore.Interfaces.Shared
{
    public interface IRoute
    {
        Task<Route> GetByCitiesIdAsync(Guid startCityId, Guid finishCityId);
        Task<Route> GetByCitiesNameAsync(string startCityName, string finishCityName);
    }
}