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
    public class KitService : IKit
    {
        private readonly AppDbContext _db;

        public KitService(AppDbContext db)
        {
            _db = db;
        }
        
        public Task<ActionResult> SendKits(CancellationToken cancellationToken)
            => Task.FromResult<ActionResult>(new OkObjectResult(new List<Kit>(_db.Kits.ToList())));
    }
}