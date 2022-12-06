using System;
using System.Collections.Generic;
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

        public async Task<ClientPackageInfo> CreateAsync(ClientPackageInfo info, string clientUserId,
            CancellationToken cancellationToken)
        {
            var user = await _dbIdentityDbContext.Users.FirstAsync(u => u.Id == clientUserId, cancellationToken);
            var client = await _db.Clients.FirstAsync(c => c.UserId == clientUserId, cancellationToken);
            var carType = await _db.CarTypes.FirstAsync(c => c.Id == info.CarType.Id, cancellationToken);
            var route = await _db.Routes.Include(r => r.StartCity)
                .Include(r => r.FinishCity)
                .FirstAsync(r => 
                    r.StartCityId == info.StartCity.Id  && 
                    r.FinishCityId == info.FinishCity.Id, cancellationToken);

            var clientPackage = new ClientPackage(info.IsSingle, info.Price)
            {
                Client = client,
                Package = info.Package,
                CarType = carType,  
                Route = route,
                OnDriverReview = new OnDriverReview()
            };
            await _db.ClientPackages.AddAsync(clientPackage, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return _mapper.Map<ClientPackageInfo>(clientPackage)
                .SetClientData(user.Name, user.Surname, user.PhoneNumber);
            }

        public async Task<ActionResult> GetWaitingClientPackage(string clientUserId,CancellationToken cancellationToken)
        {
            var clientPackageInfos = new List<ClientPackageInfo>();
            var user = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            await _db.WaitingClientPackages
                .Include(w => w.ClientPackage)
                .Include(w => w.ClientPackage.Route.StartCity)
                .Include(w => w.ClientPackage.Route.FinishCity)
                .Include(w => w.ClientPackage.Package)
                .Where(w => w.ClientPackage.Client.UserId == clientUserId)
                .ForEachAsync(WaitingList => clientPackageInfos.Add(_mapper.Map<ClientPackageInfo>(WaitingList.ClientPackage)
                    .SetClientData(user.Name, user.Surname, user.PhoneNumber)), cancellationToken);
            return new OkObjectResult(clientPackageInfos);
        }

        public async Task<ActionResult> GetOnReviewClientPackage(string clientUserId, CancellationToken cancellationToken)
        {
            var clientPackageInfos = new List<ClientPackageInfo>();
            var user = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            await _db.ClientPackages
                .Include(c => c.Route.StartCity)
                .Include(c => c.Route.FinishCity)
                .Include(c => c.Package)
                .Where(c => c.Client.UserId == clientUserId && c.OnDriverReview.OnReview)
                .ForEachAsync(ClientPackages => clientPackageInfos.Add(_mapper.Map<ClientPackageInfo>(ClientPackages)
                    .SetClientData(user.Name, user.Surname, user.PhoneNumber)), cancellationToken);
            return new OkObjectResult(clientPackageInfos);
        }
    }
}