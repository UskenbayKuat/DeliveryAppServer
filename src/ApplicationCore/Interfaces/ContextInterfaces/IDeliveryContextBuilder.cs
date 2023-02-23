using System.Linq;
using ApplicationCore.Entities.AppEntities.Orders;

namespace ApplicationCore.Interfaces.ContextInterfaces
{
    public interface IDeliveryContextBuilder
    {
        public IDeliveryContextBuilder IncludeState();
        public IDeliveryContextBuilder RouteBuilder();
        public IDeliveryContextBuilder DriverBuilder();
        public IQueryable<Delivery> Build();
    }
}