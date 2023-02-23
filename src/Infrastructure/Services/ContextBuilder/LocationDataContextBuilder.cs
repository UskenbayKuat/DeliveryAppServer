using System.Linq;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces.ContextInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ContextBuilder
{
    public class LocationDataContextBuilder : ILocationDataContextBuilder
    {
        private IQueryable<LocationData> _locationDates;
        private readonly AppDbContext _db;

        public LocationDataContextBuilder(AppDbContext db)
        {
            _db = db;
            _locationDates = _db.LocationData;
        }

        public ILocationDataContextBuilder LocationBuilder()
        {
            _locationDates = _locationDates.Include(l => l.Location);
            return this;
        }

        public IQueryable<LocationData> Build()
        {
            var locationDataList = _locationDates;
            _locationDates = _db.LocationData;
            return locationDataList;
        }
    }
}