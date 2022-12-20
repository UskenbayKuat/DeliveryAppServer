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
                .Include(o => o.Route.StartCity)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.Client)
                .Include(o => o.Delivery)
                .Include(o => o.Delivery.Orders)
                .Include(o => o.Delivery.RouteTrip)
                .Include(o => o.Delivery.RouteTrip.Driver)
                .Include(o => o.State)
                .Include(o => o.CarType)
                .Where(predicate);
        
        public async Task AddOrderToDeliveryAsync(RouteTrip routeTrip, Order order, GeneralState generalState)
        {
            order.State = await FindStateAsync((int)generalState);
            var delivery = await _db.Deliveries.FirstOrDefaultAsync(d => d.RouteTrip.Id == routeTrip.Id);
            delivery.Orders.Add(order);
            _db.Deliveries.Update(delivery);
            await _db.SaveChangesAsync();
        }
    }
}