using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Models.Dtos;
using System;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Interfaces;

namespace Infrastructure.Services.Clients
{
    public class CalculateService : ICalculate
    {
        private readonly IAsyncRepository<RoutePrice> _context;
        public CalculateService(IAsyncRepository<RoutePrice> context)
        {
            _context = context;
        }


        public async Task<CreateOrderDto> CalculateAsync(CreateOrderDto model, CancellationToken cancellationToken)
        {
            var volumeWeight = model.Package.Length * model.Package.Width * model.Package.Height / 0.005;
            var routPrice = await SetRoutePrice(model);
            var weight = volumeWeight > model.Package.Weight ? volumeWeight : model.Package.Weight;
            model.Price = AddPerKilo(routPrice, weight);
            if (model.Package.Weight > 50 || model.Package.Length > 1 || model.Package.Width > 1 || model.Package.Height > 1)
            {
                model.Price *= 1.5;
            }

            return model;
        }

        private double AddPerKilo(double price, double kilo)
        {
            return price <= 5
                ? price
                : price + 250 * ((int)kilo - 5);
        }

        private async Task<double> SetRoutePrice(CreateOrderDto model)
        {
            var route = await _context.FirstOrDefaultAsync(r => r.Route.StartCity.Name == model.StartCityName &&
                                     r.Route.FinishCity.Name == model.FinishCityName);
            return route?.Price ?? throw new ArgumentException("Не найден такой маршурт");
        }
    }
}