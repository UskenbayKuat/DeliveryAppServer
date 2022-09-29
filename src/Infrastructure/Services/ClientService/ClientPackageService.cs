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

            var package = new Package()
            {
                Height = info.Package.Height,
                Length = info.Package.Length,
                Weight = info.Package.Weight,
                Width = info.Package.Width,
                Name = info.Package.Name
            };
            await _db.Packages.AddAsync(package, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            var carType = await _db.CarTypes.FirstOrDefaultAsync(c => c.Id == info.CarType.Id, cancellationToken);
            var startCity = await _db.Cities.FirstOrDefaultAsync(c => c.Id == info.StartCity.Id, cancellationToken);
            var finishCity = await _db.Cities.FirstOrDefaultAsync(c => c.Id == info.FinishCity.Id, cancellationToken);
            var clientPackage = new ClientPackage()
            {
                Client = client,
                ClientId = client.Id,
                Package = package,
                PackageId = package.Id,
                StartCity = startCity,
                StartCityId = startCity.Id,
                FinishCity = finishCity,
                FinishCityId = finishCity.Id,
                CarType = carType,
                CarTypeId = carType.Id,
                DateTime = info.DateTime,
                IsSingle = info.IsSingle,
                Price = info.Price
            };
            await _db.ClientPackages.AddAsync(clientPackage, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return await Task.FromResult<ActionResult>(new OkObjectResult(clientPackage));
        }
    }
}