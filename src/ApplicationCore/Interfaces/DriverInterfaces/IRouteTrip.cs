using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IRouteTrip
    {
        public Task<ActionResult> GetRouteTripIsActiveAsync(string driverUserId);

        public Task<ActionResult> CreateAsync(RouteTripInfo tripInfo, string userId, CancellationToken cancellationToken);
    }
}