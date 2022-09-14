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
    public class SendCarTypesList : IGetCarTypes
    {
        private readonly AppDbContext _db;

        public SendCarTypesList(AppDbContext db)
        {
            _db = db;
        }

        public Task<ActionResult> SendCarTypes(CancellationToken cancellationToken) 
            => Task.FromResult<ActionResult>(new OkObjectResult(new List<CarType>(_db.CarTypes.ToList())));
    }
}