using System.Linq;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces.ContextInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ContextBuilder
{
    public class OrderContextBuilder : IOrderContextBuilder
    {
        private IQueryable<Order> _orders;
        private readonly AppDbContext _db;

        public OrderContextBuilder(AppDbContext db)
        {
            _db = db;
            _orders = _db.Orders;
        }

        public IOrderContextBuilder ForRejectBuilder()
        {
            _orders = _orders.Include(o => o.State)
                .Include(o => o.Delivery.Driver)
                .Include(o => o.Route)
                .Include(o => o.Delivery);
            return this;
        }
        public IOrderContextBuilder OrdersInfoBuilder()
        {
            _orders = _orders.Include(o => o.State)
                .Include(o => o.Route.StartCity)
                .Include(o => o.Client)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.CarType)
                .Include(o => o.Location);
            return this;
        }

        public IOrderContextBuilder StateBuilder()
        {
            _orders = _orders.Include(o => o.State);
            return this;
        }

        public IOrderContextBuilder ClientBuilder()
        {
            _orders = _orders.Include(o => o.Client);
            return this;
        }

        public IOrderContextBuilder ClientAndDeliveryBuilder()
        {
            _orders = _orders.Include(o => o.Delivery).Include(o => o.Delivery.Driver)
                .Include(o => o.Client);
            return this;
        }

        public IOrderContextBuilder DeliveriesInfoBuilder()
        {
            _orders = _orders.Include(o => o.Delivery)
                .Include(o => o.Delivery.Driver)
                .Include(o => o.State)
                .Include(o => o.Route.StartCity)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.CarType)
                .Include(o => o.Location);
            return this;
        }
        public IQueryable<Order> Build()
        {
            var orders = _orders;
            _orders = _db.Orders;
            return orders;
        }
    }
}