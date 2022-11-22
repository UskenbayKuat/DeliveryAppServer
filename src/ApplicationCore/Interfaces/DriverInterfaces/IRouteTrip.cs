﻿using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IRouteTrip
    {
        public Task<ActionResult> CreateRouteTrip(RouteInfo info, string userId, CancellationToken cancellationToken);
    }
}