using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.SharedInterfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.Shared
{
    public class CarTypeService : ICarType
    {
        private readonly AppDbContext _db;

        public CarTypeService(AppDbContext db)
        {
            _db = db;
        }

        public Task<ActionResult> SendCarTypes(CancellationToken cancellationToken) 
            => Task.FromResult<ActionResult>(new OkObjectResult(new List<CarType>(_db.CarTypes.ToList())));
    }
}