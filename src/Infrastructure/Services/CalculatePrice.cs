using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CalculatePrice : ICalculate
    {
        private decimal RoutePrice { get; set; }
        private double VolumeWeight { get; set; }
        
        private readonly AppDbContext _db;

        public CalculatePrice(AppDbContext db)
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
            if (kilo <= 5)
            {
                return price;
            }
            
            for (var i = 5; i <= kilo; i++)
            {
                price += 250;
            }

            return price;
        }

        private decimal SetRoutePrice(ClientPackageInfo info)
        {
            var route = _db.Routes.FirstOrDefault(r => r.StartCityId == info.StartCity.Id && r.FinishCityId == info.FinishCity.Id);
            return route?.Price ?? 0;
        } 
    }
}