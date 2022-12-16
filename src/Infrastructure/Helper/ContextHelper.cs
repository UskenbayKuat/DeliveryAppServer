using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values.Enums;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Helper
{
    public class ContextHelper
    {
        private readonly AppDbContext _db;

        public ContextHelper(AppDbContext db)
        {
            _db = db;
        }

        public Task<State> FindStateAsync(int id) =>
            _db.States.FirstAsync(s => s.Id == id) ?? throw new NullReferenceException();

        public Task<Route> FindRouteAsync(int startCityId, int finishCityId) =>
            _db.Routes
                .Include(r => r.StartCity)
                .Include(r => r.FinishCity)
                .FirstAsync(r =>
                    r.StartCityId == startCityId &&
                    r.FinishCityId == finishCityId) ?? throw new NullReferenceException();
        
        public IQueryable<RouteTrip> Trips() => _db.RouteTrips
            .Include(r => r.Driver)
            .Include(r => r.Route.StartCity)
            .Include(r => r.Route.FinishCity)
            .Where(r => r.IsActive);
        
        public Task<RouteTrip> Trip(string driverUserId) => 
            Trips().FirstOrDefaultAsync(r => r.Driver.UserId == driverUserId);

        public IQueryable<Order> Orders(Expression<Func<Order,bool>> predicate) =>
            _db.Orders
                .Include(cp => cp.Route.StartCity)
                .Include(cp => cp.Route.FinishCity)
                .Include(cp => cp.Package)
                .Include(c => c.Client)
                .Include(c => c.Delivery)
                .Include(c => c.State)
                .Where(predicate);
        
        
    }
}