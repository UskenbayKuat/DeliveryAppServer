using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.DriverInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverService
{
    public class DriverService : IDriver
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;        
        private readonly OnReviewData _onReviewData;


        public DriverService(AppDbContext db, AppIdentityDbContext identityDbContext, IMapper mapper, OnReviewData onReviewData)
        {
            _db = db;
            _identityDbContext = identityDbContext;
            _mapper = mapper;
            _onReviewData = onReviewData;
        }

        public async Task<string> FindDriverConnectionIdAsync(ClientPackageInfo clientPackageInfo,
            CancellationToken cancellationToken)
        {
            var routeTripList = await Trips()
                .Where(r => r.Route.StartCity.Id == clientPackageInfo.StartCity.Id 
                            && r.Route.FinishCity.Id == clientPackageInfo.FinishCity.Id
                            && r.CreatedAt.Day >= clientPackageInfo.CreateAt.Day)
                .ToListAsync(cancellationToken);
            var clientPackage = await ClientPackageAsync(clientPackageInfo.ClientPackageId, cancellationToken);
            foreach (var routeTrip in routeTripList)
            {
                var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == routeTrip.Driver.UserId, cancellationToken);
                if (string.IsNullOrEmpty(chatHub?.ConnectionId)) continue;
                if (await CheckRejectedAsync(routeTrip.Id, clientPackageInfo.ClientPackageId)) continue;
                await UpDateClientPackageStateAsync(clientPackage, ClientPackageState.OnReview);
                if (_onReviewData.ReviewDictionary.ContainsKey(routeTrip.Id))
                {
                    _onReviewData.ReviewDictionary.GetValueOrDefault(routeTrip.Id)?.Add(clientPackageInfo);
                }
                else
                {
                    _onReviewData.ReviewDictionary.Add(routeTrip.Id, new List<ClientPackageInfo> {clientPackageInfo});
                }
                return chatHub.ConnectionId;
            }
            await _db.WaitingClientPackages.AddAsync(new WaitingClientPackage { ClientPackage = clientPackage.ChangeState(ClientPackageState.PendingForReview) }, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return string.Empty;
        }

        public async Task<List<ClientPackageInfo>> FindClientPackagesAsync(string driverUserId)
        {
            var clientPackagesInfo = new List<ClientPackageInfo>();
            var routeTrip = await Trip(driverUserId) ?? throw new HubException();
            var waitingClientPackages = await WaitingClientPackages(routeTrip.Route.Id, routeTrip.CreatedAt)
                .Where(cp=>cp.ClientPackage.ClientPackageState == ClientPackageState.PendingForReview).ToListAsync();
            _db.WaitingClientPackages.RemoveRange(waitingClientPackages);
            foreach(var clientPackage in waitingClientPackages.Select(w => w.ClientPackage))
            {
                await UpDateClientPackageStateAsync(clientPackage, ClientPackageState.OnReview);
                var user = await _identityDbContext.Users.FirstOrDefaultAsync(u =>u.Id == clientPackage.Client.UserId);
                clientPackagesInfo.Add(_mapper.Map<ClientPackageInfo>(clientPackage).SetClientData(user.Name, user.Surname, user.PhoneNumber));
            }
            _onReviewData.ReviewDictionary.Add(routeTrip.Id, clientPackagesInfo);
            return clientPackagesInfo;
        }
        
        public async Task<ActionResult> SendClientPackagesToDriverAsync(string driverUserId)
        {
            try
            {
                var routeTrip = await Trip(driverUserId) ?? throw new NullReferenceException("Для проверки заказов создайте поездку");
                return new OkObjectResult(_onReviewData.ReviewDictionary.GetValueOrDefault(routeTrip.Id));
            }
            catch (NullReferenceException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<ActionResult> SendRouteTripToDriverAsync(string driverUserId)
        {
            try
            {
                var routeTrip = await Trip(driverUserId) ?? throw new NullReferenceException("Текущих поездок нет");
                return new OkObjectResult(_mapper.Map<RouteTripInfo>(routeTrip));
            }
            catch (NullReferenceException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<string> RejectNextFindDriverConnectionIdAsync(string driverUserId,
            ClientPackageInfo clientPackageInfo,
            CancellationToken cancellationToken)
        {
            var clientPackage = await ClientPackageAsync(clientPackageInfo.ClientPackageId, cancellationToken);
            var routeTrip = await Trip(driverUserId);
            await _db.RejectedClientPackages
                .AddAsync(new RejectedClientPackage
                {
                    ClientPackage = clientPackage, 
                    RouteTrip = routeTrip
                }, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return await FindDriverConnectionIdAsync(clientPackageInfo, cancellationToken);
        }

        private async Task<bool> CheckRejectedAsync(int routeTripId, int clientPackageId)
        => await _db.RejectedClientPackages
                .AnyAsync(r =>
                    r.RouteTrip.Id == routeTripId &&
                    r.ClientPackage.Id == clientPackageId);

        private async Task UpDateClientPackageStateAsync(ClientPackage clientPackage, ClientPackageState state)
        {
            _db.ClientPackages.Update(clientPackage.ChangeState(state));
            await _db.SaveChangesAsync();
        }

        private async Task<ClientPackage> ClientPackageAsync(int clientPackageId, CancellationToken cancellationToken) =>  await _db.ClientPackages
            .Include(cp=>cp.Route.StartCity)
            .Include(cp=>cp.Route.FinishCity)
            .Include(cp=>cp.Package)
            .Include(c => c.Client)
            .FirstAsync(c => c.Id == clientPackageId, cancellationToken);
        
        private IQueryable<RouteTrip> Trips() => _db.RouteTrips
            .Include(r => r.Driver)
            .Include(r => r.Route.StartCity)
            .Include(r => r.Route.FinishCity)
            .Where(r => r.IsActive);

        private Task<RouteTrip> Trip(string userDriverId) => 
            Trips().FirstOrDefaultAsync(r => r.Driver.UserId == userDriverId);

        private IQueryable<WaitingClientPackage> WaitingClientPackages(int id, DateTime deliveryDate) => _db.WaitingClientPackages
            .Include(w => w.ClientPackage.Client)
            .Include(w => w.ClientPackage.Package)
            .Include(w => w.ClientPackage.Route.StartCity)
            .Include(w => w.ClientPackage.Route.FinishCity)
            .Where(w =>
                w.ClientPackage.Route.Id == id &&
                w.ClientPackage.CreatedAt.Day <= deliveryDate.Day);
    }
}