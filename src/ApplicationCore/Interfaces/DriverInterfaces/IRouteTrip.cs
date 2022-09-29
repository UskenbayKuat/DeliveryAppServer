using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IRouteTrip
    {
        public Task<ActionResult> CreateRouteTrip(RouteInfo info, CancellationToken cancellationToken);
    }
}