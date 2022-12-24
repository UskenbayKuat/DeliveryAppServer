using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values.Enums;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;


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

        public IIncludableQueryable<Order, City> OrdersForReject() =>
            _db.Orders
                .Include(o => o.Route.StartCity)
                .Include(o => o.Route.FinishCity);
        
        public IIncludableQueryable<Order, Client> OrdersForOrderInfo() =>
            OrdersForReject()
                .Include(o => o.State)
                .Include(o => o.Package)
                .Include(o => o.CarType)
                .Include(o => o.Location)
                .Include(o => o.Client);
        
        public IIncludableQueryable<Order, RouteTrip> OrdersForOrderInfoWithDriver() =>
            OrdersForOrderInfo()
                .Include(o => o.Delivery.RouteTrip);

        public IIncludableQueryable<Order, Driver> OrdersForDriverInfo() =>
            OrdersForOrderInfoWithDriver()
                .Include(o => o.Delivery)
                .Include(o => o.Delivery.RouteTrip.Driver);
        
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