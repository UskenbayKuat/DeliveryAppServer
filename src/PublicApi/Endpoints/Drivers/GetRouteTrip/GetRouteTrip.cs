using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Drivers.GetRouteTrip
{
    public class GetRouteTrip : EndpointBaseAsync.WithRequest<int>.WithActionResult<GetRouteTripResult>
    {
        private readonly IGetRouteTrip _getRouteTrip;

        public GetRouteTrip(IGetRouteTrip getRouteTrip)
        {
            _getRouteTrip = getRouteTrip;
        }

        [HttpGet("api/drivers/GetRouteTrip/{id:int}")]
        public override async Task<ActionResult<GetRouteTripResult>> HandleAsync([FromRoute]int id, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _getRouteTrip.SendRoute(id, cancellationToken);
        }
    }
}