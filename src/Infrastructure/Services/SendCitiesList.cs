using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services
{
    public class SendCitiesList : IGetCities
    {
        private readonly AppDbContext _db;

        public SendCitiesList(AppDbContext db)
        {
            _db = db;
        }

        public Task<ActionResult> SendCities(CancellationToken cancellationToken) 
            => Task.FromResult<ActionResult>(new OkObjectResult(new List<City>(_db.Cities.ToList())));
    }
}