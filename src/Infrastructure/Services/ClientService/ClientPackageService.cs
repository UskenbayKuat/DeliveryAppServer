using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientService
{
    public class ClientPackageService : IClientPackage
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _dbIdentityDbContext;
        private readonly IMapper _mapper;

        public ClientPackageService(AppDbContext db, AppIdentityDbContext dbIdentityDbContext, IMapper mapper)
        {
            _db = db;
            _dbIdentityDbContext = dbIdentityDbContext;
            _mapper = mapper;
        }

        public async Task<ClientPackageInfoToDriver> CreateAsync(ClientPackageInfo info, string clientUserId,
            CancellationToken cancellationToken)
        {
            var user = await _dbIdentityDbContext.Users.FirstAsync(u => u.Id == clientUserId, cancellationToken);
            var client = await _db.Clients.FirstAsync(c => c.UserId == clientUserId, cancellationToken);
            var carType = await _db.CarTypes.FirstAsync(c => c.Id == info.CarTypeId, cancellationToken);
            var route = await _db.Routes.Include(r => r.StartCity)
                .Include(r => r.FinishCity)
                .FirstAsync(r => r.StartCityId == info.StartCityId  && r.FinishCityId == info.FinishCityId, cancellationToken);

            var clientPackage = new ClientPackage(info.IsSingle, info.Price)
            {
                Client = client,
                Package = info.Package,
                CarType = carType,  
                Route = route
            };
            await _db.ClientPackages.AddAsync(clientPackage, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ClientPackageInfoToDriver>(clientPackage)
                .SetClientData(user.Name, user.Surname, user.PhoneNumber);
        }

    }
}