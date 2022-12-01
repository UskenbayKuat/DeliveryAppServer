using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.SharedInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.Shared
{
    public class DriverAppDataService : IDeliveryAppData<DriverAppDataInfo>
    {
        private readonly AppDbContext _db;

        public DriverAppDataService(AppDbContext db)
        {
            _db = db;
        }
        public Task<ActionResult> SendData(CancellationToken cancellationToken)
        {
            var info = new DriverAppDataInfo()
            {
                Cities = _db.Cities.ToList(),
                Kits = _db.Kits.ToList(),
                CarBrands = _db.CarBrands.ToList(),
                CarTypes = _db.CarTypes.ToList(),
                CarColors = _db.CarColors.ToList()
            };
            return Task.FromResult<ActionResult>(new OkObjectResult(info));
        }
    }
}