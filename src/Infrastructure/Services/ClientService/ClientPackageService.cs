using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.ClientInterfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientService
{
    public class ClientPackageService : IClientPackage
    {
        
        private readonly AppDbContext _db;

        public ClientPackageService(AppDbContext db)
        {
            _db = db;
        }
        
        public async Task<ActionResult> CreateClientPackage(ClientPackageInfo info, CancellationToken cancellationToken)
        {
            var client = await _db.Clients.FirstOrDefaultAsync(c => c.Id == info.ClientId, cancellationToken);
            if (client is null)
            {
                return await Task.FromResult<ActionResult>(new BadRequestResult());
            }

            var package = new Package
            {
                Height = info.Package.Height,
                Length = info.Package.Length,
                Weight = info.Package.Weight,
                Width = info.Package.Width,
                Name = info.Package.Name
            };
            await _db.Packages.AddAsync(package, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            var carType = await _db.CarTypes.FirstOrDefaultAsync(c => c.Id == info.CarTypeId, cancellationToken);
            
            var route = await _db.Routes.
                FirstOrDefaultAsync(r => r.StartCityId == info.StartCityId 
                                         && r.FinishCityId == info.FinishCityId, cancellationToken);
            var routeDate = new RouteDate
            {
                Route = route,
                CreateDateTime = info.DateTime
            };
            var clientPackage = new ClientPackage
            {
                Client = client,
                Package = package,
                CarType = carType,
                IsSingle = info.IsSingle,
                Price = info.Price,
                RouteDate = routeDate
            };
            await _db.RouteDate.AddAsync(routeDate, cancellationToken);
            await _db.ClientPackages.AddAsync(clientPackage, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult<ActionResult>(new OkObjectResult(clientPackage));
        }
    }
}