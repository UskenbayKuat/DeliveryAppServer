using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Routes;
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
        
        public async Task<ActionResult> CreateClientPackage(ClientPackageInfo info, string userId, CancellationToken cancellationToken)
        {
            try
            {
                var client = await _db.Clients.FirstAsync(c => c.UserId == userId, cancellationToken);
                var carType = await _db.CarTypes.FirstAsync(c => c.Id == info.CarTypeId, cancellationToken);
                var route = await _db.Routes.
                    FirstAsync(r => r.StartCityId == info.StartCityId 
                                             && r.FinishCityId == info.FinishCityId, cancellationToken);
                var clientPackage = new ClientPackage(info.IsSingle, info.Price)
                    .AddClientPackageData(carType,client, info.Package, null, new RouteDate(info.DateTime).AddRoute(route));
                
                await _db.ClientPackages.AddAsync(clientPackage, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                return await Task.FromResult<ActionResult>(new OkObjectResult(clientPackage));
            }
            catch
            {
                return await Task.FromResult<ActionResult>(new BadRequestResult());
            }
        }
    }
}