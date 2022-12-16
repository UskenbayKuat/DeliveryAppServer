using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.SharedInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Shared
{
    public class ClientAppDataService : IDeliveryAppData<ClientAppDataInfo>
    {
        private readonly AppDbContext _db;
        public ClientAppDataService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<ActionResult> SendDataAsync(CancellationToken cancellationToken)
        {
            var cities =await _db.Cities.ToListAsync(cancellationToken);
            var carTypes =await  _db.CarTypes.ToListAsync(cancellationToken);
            var info = new ClientAppDataInfo()
            {
                Cities = cities,
                CarTypes = carTypes
            };
            return new OkObjectResult(info);
        }
    }
}