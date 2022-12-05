using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.OrderInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.OrderServices
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;

        public OrderService(AppDbContext db, AppIdentityDbContext identityDbContext, IMapper mapper)
        {
            _db = db;
            _identityDbContext = identityDbContext;
            _mapper = mapper;
        }

        public async Task<string> CreateAsync(string driverUserId, int clientPackageId)
        {
            try
            {
                var clientPackage = await _db.ClientPackages.Include(c => c.Client).FirstAsync(c => c.Id == clientPackageId);
                var routeTrip = await _db.RouteTrips.Include(r => r.Driver)
                    .FirstAsync(r => r.Driver.UserId == driverUserId && r.IsActive);
                clientPackage.Order = await _db.Orders
                    .FirstOrDefaultAsync(o => o.RouteTrip.Id == routeTrip.Id && o.OrderState == OrderState.New) 
                                      ?? new Order(DateTime.Now) { RouteTrip = routeTrip, OrderState = OrderState.New };
                _db.ClientPackages.Update(clientPackage);
                await _db.SaveChangesAsync();
                return (await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == clientPackage.Client.UserId))?.ConnectionId;
            }
            catch
            {
                throw new HubException();
            }
        }


        public async Task<ActionResult> GetActiveOrdersForClient(string userId, CancellationToken cancellationToken)
        {
            var orderInfoList = new List<OrderInfo>();
            var userClient = await _identityDbContext.Users.FirstAsync(u => u.Id == userId, cancellationToken);
            var clientPackageList = await ClientPackages(userClient.Id, cancellationToken);
            foreach (var clientPackage in clientPackageList)
            {
                var userDriver = await _identityDbContext.Users.FirstAsync(u => u.Id == clientPackage.Order.RouteTrip.Driver.UserId, cancellationToken);
                var orderInfo = _mapper.Map<OrderInfo>(clientPackage).SetDriverData(userDriver.Name, userDriver.Surname, userDriver.PhoneNumber);
                orderInfo.SetClientData(userClient.Name, userClient.Surname);
                orderInfoList.Add(orderInfo);
            }
            return new OkObjectResult(orderInfoList);
        }
        
        
        private async Task<List<ClientPackage>> ClientPackages(string userClientId, CancellationToken cancellationToken) => await _db.ClientPackages
            .Include(c => c.Order.RouteTrip.Driver)
            .Include(c => c.Route.StartCity)
            .Include(c => c.Route.FinishCity)
            .Include(c => c.Order)
            .Include(c => c.Package)
            .Where(c => c.Client.UserId == userClientId && c.Order.RouteTrip.IsActive).ToListAsync(cancellationToken);
        
    }
}