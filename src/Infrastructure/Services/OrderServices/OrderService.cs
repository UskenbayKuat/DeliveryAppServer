using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces.OrderInterfaces;
using AutoMapper;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.OrderServices
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDbContext;

        public OrderService(AppDbContext db, AppIdentityDbContext identityDbContext)
        {
            _db = db;
            _identityDbContext = identityDbContext;
        }

        public async Task<string> FindDriverConnectionIdAsync(ClientPackageInfoToDriver clientPackageInfoToDriver, CancellationToken cancellationToken)
        {
            var routeTripList = _db.RouteTrips.Include(r => r.RouteDate) // 1 - false, 2 - false, 3 -clientpackage
                .Include(r => r.Driver)
                .Include(r => r.RouteDate.Route)
                .Where(r => r.RouteDate.Route.Id == clientPackageInfoToDriver.Route.Id
                            && r.IsActive == true && r.RouteDate.DeliveryDate.Day >= clientPackageInfoToDriver.DateTime.Day).ToList();
            for (var i = 0; i < routeTripList.Count; i++)
            {
                var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == routeTripList[i].Driver.UserId,
                    cancellationToken);
                if (!string.IsNullOrEmpty(chatHub.ConnectionId))
                {
                    var checkRefusal = await _db.RejectedClientPackages.AnyAsync(
                        r => r.RouteTrip.Id == routeTripList[i].Id && r.ClientPackage.Id == clientPackageInfoToDriver.ClientPackageId,
                        cancellationToken);
                    if (!checkRefusal) //false
                    {
                        return chatHub.ConnectionId;
                    }
                }
            }

            var clientPackage =
                await _db.ClientPackages.FirstAsync(c => c.Id == clientPackageInfoToDriver.ClientPackageId, cancellationToken);
            var waitingList = new WaitingList { ClientPackage = clientPackage };
            await _db.WaitingList.AddAsync(waitingList, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return string.Empty;
        }

        public async Task<List<ClientPackageInfoToDriver>> FindClientPackagesAsync(string userId)
        {
            var orderInfo = new List<ClientPackageInfoToDriver>();
            var routeTrip = await _db.RouteTrips.Include(d => d.Driver)
                                .Include(r => r.RouteDate)
                                .Include(r => r.RouteDate.Route)
                                .FirstOrDefaultAsync(r =>
                                    r.Driver.UserId == userId && r.IsActive == true)
                            ?? throw new HubException();
            var waitingLists = _db.WaitingList.Include(w => w.ClientPackage)
                .Include(w => w.ClientPackage.Client)
                .Include(w => w.ClientPackage.Package)
                .Include(w => w.ClientPackage.RouteDate)
                .Include(w => w.ClientPackage.RouteDate.Route)
                .Include(w => w.ClientPackage.RouteDate.Route.StartCity)
                .Include(w => w.ClientPackage.RouteDate.Route.FinishCity)
                .Where(w =>
                    w.ClientPackage.RouteDate.Route.Id == routeTrip.RouteDate.Route.Id &&
                    w.ClientPackage.RouteDate.DeliveryDate.Day <= routeTrip.RouteDate.DeliveryDate.Day).ToList();
            if (waitingLists.Count <= 0)
            {
                return orderInfo;
            }
            for (var i = 0; i < waitingLists.Count; i++)
            {
                var user = await _identityDbContext.Users.FirstOrDefaultAsync(u =>
                    u.Id == waitingLists[i].ClientPackage.Client.UserId) 
                           ?? throw new HubException();
                orderInfo.Add(new ClientPackageInfoToDriver
                {
                    ClientPackageId = waitingLists[i].ClientPackage.Id,
                    Location = null,
                    Package = waitingLists[i].ClientPackage.Package,
                    Price = waitingLists[i].ClientPackage.Price,
                    FullName = user.Surname + " " + user.Name,
                    PhoneNumber = user.PhoneNumber,
                    IsSingle = waitingLists[i].ClientPackage.IsSingle,
                    Route = waitingLists[i].ClientPackage.RouteDate.Route,
                    DateTime = waitingLists[i].ClientPackage.RouteDate.DeliveryDate
                });
            }
            return orderInfo;
        }

        public async Task<string> CreateAsync(string driverUserId, int clientPackageId)
        {
            try
            {
                var clientPackage = await _db.ClientPackages.Include(c => c.Client)
                    .FirstAsync(c => c.Id == clientPackageId);
                var routeTrip = await _db.RouteTrips.Include(r => r.Driver)
                    .FirstAsync(r => r.Driver.UserId == driverUserId && r.IsActive == true);

                var status = await _db.Statuses.FirstAsync(s => s.State == State.New.ToString());
                var order = await _db.Orders.Include(o => o.RouteTrip)
                    .Include(o => o.Status)
                    .FirstOrDefaultAsync(o => o.RouteTrip.Id == routeTrip.Id && o.Status.State == status.State);
                if (order is null)
                {
                    var newOrder = new Order(DateTime.Now) { RouteTrip = routeTrip, Status = status };
                    newOrder.ClientPackages.Add(clientPackage);
                    await _db.Orders.AddAsync(newOrder);
                }
                else
                {
                    order.ClientPackages.Add(clientPackage);
                    _db.Orders.Update(order);
                }

                await _db.SaveChangesAsync();
                var chatHub = await _db.ChatHubs.FirstAsync(c => c.UserId == clientPackage.Client.UserId);
                return chatHub.ConnectionId;
            }
            catch
            {
                throw new HubException();
            }
        }

        public async Task<string> RejectAsync(string driverUserId, ClientPackageInfoToDriver clientPackageInfoToDriver,
            CancellationToken cancellationToken)
        {
            var clientPackage = await _db.ClientPackages.Include(c => c.Client)
                .FirstAsync(c => c.Id == clientPackageInfoToDriver.ClientPackageId, cancellationToken);
            var routeTrip = await _db.RouteTrips.Include(r => r.Driver)
                .FirstAsync(r => r.Driver.UserId == driverUserId && r.IsActive == true, cancellationToken);
            var refusalOrder = new RejectedClientPackage { ClientPackage = clientPackage, RouteTrip = routeTrip };
            await _db.RejectedClientPackages.AddAsync(refusalOrder, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return await FindDriverConnectionIdAsync(clientPackageInfoToDriver, cancellationToken);
        }
    }
}