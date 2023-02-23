using System.Linq;
using ApplicationCore.Entities.AppEntities.Orders;

namespace ApplicationCore.Interfaces.ContextInterfaces
{
    public interface IOrderContextBuilder
    {
        public IOrderContextBuilder OrdersInfoBuilder();
        public IOrderContextBuilder StateBuilder();
        public IOrderContextBuilder ClientBuilder();
        public IOrderContextBuilder DeliveriesInfoBuilder();
        public IOrderContextBuilder ForRejectBuilder();
        public IOrderContextBuilder ClientAndDeliveryBuilder();
        public IQueryable<Order> Build();
    }
}