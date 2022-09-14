using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class SendRouteTrip : IGetRouteTrip
    {
        private readonly AppDbContext _db;

        public SendRouteTrip(AppDbContext db)
        {
            _db = db;
        }
        public async Task<ActionResult> SendRoute(int driverId, CancellationToken cancellationToken)
        {
            var routeTrip = await _db.RouteTrips
                .Include(c =>  c.StartCity)
                .Include(c => c.FinishCity)
                .FirstOrDefaultAsync(r => r.DriverId == driverId, cancellationToken);
            return new OkObjectResult(routeTrip);
        }
    }
}