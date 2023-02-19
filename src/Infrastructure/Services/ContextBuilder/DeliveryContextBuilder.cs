using System.Linq;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces.ContextInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ContextBuilder
{
    public class DeliveryContextBuilder : IDeliveryContextBuilder
    {
        private IQueryable<Delivery> _deliveries;
        private readonly AppDbContext _db;

        public DeliveryContextBuilder(AppDbContext dbContext)
        {
            _db = dbContext;
            _deliveries = _db.Deliveries;
        }
        
        public IDeliveryContextBuilder IncludeState()
        {
            _deliveries = _deliveries.Include(d => d.State);
            return this;
        }

        public IDeliveryContextBuilder RouteBuilder()
        {
            _deliveries = _deliveries.Include(d => d.Route.StartCity)
                .Include(d => d.Route.FinishCity);
            return this;
        }

        public IDeliveryContextBuilder DriverBuilder()
        {
            _deliveries = _deliveries.Include(d => d.Driver);
            return this;
        }

        public IQueryable<Delivery> Build()
        {
            var deliveries = _deliveries;
            _deliveries = _db.Deliveries;
            return deliveries;
        }
    }
}