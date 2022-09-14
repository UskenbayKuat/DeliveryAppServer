using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces
{
    public interface IGetRouteTrip
    {
        public Task<ActionResult> SendRoute(int driverId, CancellationToken cancellationToken);
    }
}