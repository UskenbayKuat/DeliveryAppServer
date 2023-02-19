using System.Linq;
using ApplicationCore.Entities.AppEntities;

namespace ApplicationCore.Interfaces.ContextInterfaces
{
    public interface IDriverContextBuilder
    {
        public IDriverContextBuilder CarBuilder();
        public IQueryable<Driver> Build();
    }
}