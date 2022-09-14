using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces
{
    public interface IRouteTrip
    {
        public Task<ActionResult> CreateRouteTrip(RouteInfo info, CancellationToken cancellationToken);
    }
}