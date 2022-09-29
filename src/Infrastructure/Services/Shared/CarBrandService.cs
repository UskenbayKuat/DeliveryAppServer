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
    public class CarBrandService : ICarBrand
    {
        private readonly AppDbContext _db;

        public CarBrandService(AppDbContext db)
        {
            _db = db;
        }

        public Task<ActionResult> SendCarBrands(CancellationToken cancellationToken) 
            => Task.FromResult<ActionResult>(new OkObjectResult(new List<CarBrand>(_db.CarBrands.ToList())));
    }
}