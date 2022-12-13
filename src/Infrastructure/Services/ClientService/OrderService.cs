using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientService
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _dbIdentityDbContext;
        private readonly IMapper _mapper;
        private readonly StateHelper _stateHelper;

        public OrderService(AppDbContext db, AppIdentityDbContext dbIdentityDbContext, IMapper mapper, StateHelper stateHelper)
        {
            _db = db;
            _dbIdentityDbContext = dbIdentityDbContext;
            _mapper = mapper;
            _stateHelper = stateHelper;
        }

        public async Task<OrderInfo> CreateAsync(OrderInfo info, string clientUserId,
            CancellationToken cancellationToken)
        {
            var user = await _dbIdentityDbContext.Users.FirstAsync(u => u.Id == clientUserId, cancellationToken);
            var client = await _db.Clients.FirstAsync(c => c.UserId == clientUserId, cancellationToken);
            var carType = await _db.CarTypes.FirstAsync(c => c.Id == info.CarType.Id, cancellationToken);
            var route = await _db.Routes
                .Include(r => r.StartCity)
                .Include(r => r.FinishCity)
                .FirstAsync(r => 
                    r.StartCity.Id == info.StartCity.Id  && 
                    r.FinishCity.Id == info.FinishCity.Id, cancellationToken);
            var order = new Order(info.IsSingle, info.Price, info.DeliveryDate)
            {
                Client = client,
                Package = info.Package,
                CarType = carType,  
                Route = route,
                State =  _stateHelper.FindState((int)GeneralState.New)
            };
            await _db.Orders.AddAsync(order, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return _mapper.Map<OrderInfo>(order)
                .SetClientData(user.Name, user.Surname, user.PhoneNumber);
            }

        public async Task<ActionResult> GetWaitingOrdersAsync(string clientUserId,CancellationToken cancellationToken)
        {
            var ordersInfo = new List<OrderInfo>();
            var user = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            await _db.Orders
                .Include(o => o.Route.StartCity)
                .Include(o => o.Route.FinishCity)
                .Include(o => o.Package)
                .Include(o => o.CarType)
                .Where(o => o.Client.UserId == clientUserId)
                .ForEachAsync(o => ordersInfo.Add(_mapper.Map<OrderInfo>(o)
                    .SetClientData(user.Name, user.Surname, user.PhoneNumber)), cancellationToken);
            return new OkObjectResult(ordersInfo);
        }

        public async Task<ActionResult> GetOnReviewOrdersAsync(string clientUserId, CancellationToken cancellationToken)
        {
            var ordersInfo = new List<OrderInfo>();
            var user = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            await _db.Orders
                .Include(c => c.Route.StartCity)
                .Include(c => c.Route.FinishCity)
                .Include(c => c.Package)
                .Include(c => c.CarType)
                .Where(c => c.Client.UserId == clientUserId && c.State == _stateHelper.FindState((int)GeneralState.OnReview))
                .ForEachAsync(cp => ordersInfo.Add(_mapper.Map<OrderInfo>(cp)
                    .SetClientData(user.Name, user.Surname, user.PhoneNumber)), cancellationToken);
            return new OkObjectResult(ordersInfo);
        }
    }
}