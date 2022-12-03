using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.DriverInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverService
{
    public class DriverService : IDriver
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;

        public DriverService(AppDbContext db, AppIdentityDbContext identityDbContext, IMapper mapper)
        {
            _db = db;
            _identityDbContext = identityDbContext;
            _mapper = mapper;
        }

        public async Task<string> FindDriverConnectionIdAsync(ClientPackageInfo clientPackageInfo,
            CancellationToken cancellationToken)
        {
            var routeTripList = await Trips()
                .Where(r => r.Route.StartCity.Id == clientPackageInfo.StartCity.Id 
                            && r.Route.FinishCity.Id == clientPackageInfo.FinishCity.Id
                            && r.CreatedAt.Day >= clientPackageInfo.CreateAt.Day)
                .ToListAsync(cancellationToken);
            
            foreach (var routeTrip in routeTripList)
            {
                var chatHub =
                    await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == routeTrip.Driver.UserId,
                        cancellationToken);
                if (string.IsNullOrEmpty(chatHub?.ConnectionId)) continue;
                var checkRefusal = await _db.RejectedClientPackages
                    .AnyAsync(r =>
                        r.RouteTrip.Id == routeTrip.Id &&
                        r.ClientPackage.Id == clientPackageInfo.ClientPackageId, cancellationToken);
                if (!checkRefusal) return chatHub.ConnectionId;
            }
            var clientPackage = await ClientPackage(clientPackageInfo.ClientPackageId, cancellationToken);
            await _db.WaitingList.AddAsync(new WaitingList { ClientPackage = clientPackage }, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return string.Empty;
        }

        public async Task<List<ClientPackageInfo>> FindClientPackagesAsync(string userId)
        {
            var clientPackageInfos = new List<ClientPackageInfo>();
            var routeTrip = await Trips().FirstAsync(r => r.Driver.UserId == userId)
                            ?? throw new HubException();
            var waitingLists = await WaitingLists(routeTrip.Route.Id, routeTrip.CreatedAt).ToListAsync();
            // if (waitingLists.Count <= 0) TODO ?? need to check without working(delete)
            // {
            //     return clientPackageInfoToDrivers;
            // }
            foreach(var waitingList in waitingLists)
            {
                var user = await _identityDbContext.Users.FirstOrDefaultAsync(u =>
                    u.Id == waitingList.ClientPackage.Client.UserId);
                var clientPackageInfo = _mapper.Map<ClientPackageInfo>(waitingList.ClientPackage);
                clientPackageInfo.SetClientData(user.Name, user.Surname, user.PhoneNumber);
                clientPackageInfos.Add(clientPackageInfo);
            }

            return clientPackageInfos;
        }

        public async Task<string> RejectNextFindDriverConnectionIdAsync(string driverUserId,
            ClientPackageInfo clientPackageInfo,
            CancellationToken cancellationToken)
        {
            var clientPackage = await ClientPackage(clientPackageInfo.ClientPackageId, cancellationToken);
            var routeTrip = await Trips() .FirstAsync(r => r.Driver.UserId == driverUserId, cancellationToken);
            await _db.RejectedClientPackages
                .AddAsync(new RejectedClientPackage
                {
                    ClientPackage = clientPackage, 
                    RouteTrip = routeTrip
                }, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return await FindDriverConnectionIdAsync(clientPackageInfo, cancellationToken);
        }
        
        
        private async Task<ClientPackage> ClientPackage(int clientPackageId, CancellationToken cancellationToken) =>  await _db.ClientPackages
            .Include(c => c.Client)
            .FirstAsync(c => c.Id == clientPackageId, cancellationToken);
        
        private IQueryable<RouteTrip> Trips() => _db.RouteTrips
            .Include(r => r.Driver)
            .Include(r => r.Route)
            .Where(r => r.IsActive);

        private IQueryable<WaitingList> WaitingLists(int id, DateTime deliveryDate) => _db.WaitingList
            .Include(w => w.ClientPackage.Client)
            .Include(w => w.ClientPackage.Package)
            .Include(w => w.ClientPackage.Route.StartCity)
            .Include(w => w.ClientPackage.Route.FinishCity)
            .Where(w =>
                w.ClientPackage.Route.Id == id &&
                w.ClientPackage.CreatedAt.Day <= deliveryDate.Day);
    }
}