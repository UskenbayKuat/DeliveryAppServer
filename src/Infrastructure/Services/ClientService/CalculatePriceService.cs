using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientService
{
    public class CalculatePriceService : ICalculate
    {
        private decimal RoutePrice { get; set; }
        private double VolumeWeight { get; set; }
        
        private readonly AppDbContext _db;

        public CalculatePriceService(AppDbContext db)
        {
            _db = db;
        }


        public async Task<ActionResult> Calculate(ClientPackageInfo info,CancellationToken cancellationToken)
        {
            VolumeWeight = info.Package.Length * info.Package.Width * info.Package.Height / 0.005;
            RoutePrice = SetRoutePrice(info);
            var weight = VolumeWeight > info.Package.Weight ? VolumeWeight : info.Package.Weight;
            info.Price = AddPerKilo(RoutePrice, weight);
            if (info.Package.Weight > 50 || info.Package.Length > 1 || info.Package.Width > 1 || info.Package.Height > 1)
            {
                info.Price *= (decimal)1.5;
            }

            return  await Task.FromResult<ActionResult>(new OkObjectResult(info));
        }

        private decimal AddPerKilo(decimal price, double kilo)
        {
            return price <= 5 
                ? price 
                : price + 250 * (decimal)((int)kilo - 5);
        }

        private decimal SetRoutePrice(ClientPackageInfo info)
        {
            var routePrice = _db.RoutePrice.Include(r => r.Route)
                .FirstOrDefault(r => r.Route.StartCityId == info.StartCityId &&
                                     r.Route.FinishCityId == info.FinishCityId);
            return routePrice?.Price ?? 0;
        } 
    }
}