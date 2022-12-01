using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.SharedInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.Shared
{
    public class ClientAppDataService : IDeliveryAppData<ClientAppDataInfo>
    {
        private readonly AppDbContext _db;
        public ClientAppDataService(AppDbContext db)
        {
            _db = db;
        }
        public Task<ActionResult> SendData(CancellationToken cancellationToken)
        {
            var info = new ClientAppDataInfo()
            {
                Cities = _db.Cities.ToList(),
                CarTypes = _db.CarTypes.ToList(),
            };
            return Task.FromResult<ActionResult>(new OkObjectResult(info));
        }
    }
}