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
    public class CarColorService : ICarColor
    {
        private readonly AppDbContext _db;

        public CarColorService(AppDbContext db)
        {
            _db = db;
        }

        public Task<ActionResult> SendCarColors(CancellationToken cancellationToken) 
            => Task.FromResult<ActionResult>(new OkObjectResult(new List<CarColor>(_db.CarColors.ToList())));
    }
}