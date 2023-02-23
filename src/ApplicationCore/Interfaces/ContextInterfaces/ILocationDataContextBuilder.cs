using System.Linq;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;

namespace ApplicationCore.Interfaces.ContextInterfaces
{
    public interface ILocationDataContextBuilder
    {
        public ILocationDataContextBuilder LocationBuilder();
        public IQueryable<LocationData> Build();

    }
}