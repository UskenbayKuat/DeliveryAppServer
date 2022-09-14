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
    public class SendKitsList : IGetKits
    {
        private readonly AppDbContext _db;

        public SendKitsList(AppDbContext db)
        {
            _db = db;
        }
        
        public Task<ActionResult> SendKits(CancellationToken cancellationToken)
            => Task.FromResult<ActionResult>(new OkObjectResult(new List<Kit>(_db.Kits.ToList())));
    }
}