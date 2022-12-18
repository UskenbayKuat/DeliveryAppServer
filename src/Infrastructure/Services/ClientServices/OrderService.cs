using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientServices
{
    public class OrderService : IOrder
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _dbIdentityDbContext;
        private readonly IMapper _mapper;
        private readonly ContextHelper _contextHelper;
        private readonly IDriver _driverService;

        public OrderService(AppDbContext db, AppIdentityDbContext dbIdentityDbContext, IMapper mapper, ContextHelper contextHelper, IDriver driverService)
        {
            _db = db;
            _dbIdentityDbContext = dbIdentityDbContext;
            _mapper = mapper;
            _contextHelper = contextHelper;
            _driverService = driverService;
        }

        public async Task<ActionResult> CreateAsync(OrderInfo info, string clientUserId, Func<string, Task> func,
            CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dbIdentityDbContext.Users.FirstAsync(u => u.Id == clientUserId, cancellationToken);
                var client = await _db.Clients.FirstAsync(c => c.UserId == clientUserId, cancellationToken);
                var carType = await _db.CarTypes.FirstAsync(c => c.Id == info.CarType.Id, cancellationToken);
                var route = await _contextHelper.FindRouteAsync(info.StartCity.Id, info.FinishCity.Id);
                var state = await _contextHelper.FindStateAsync((int)GeneralState.New);
                var order = new Order(info.IsSingle, info.Price, info.DeliveryDate)
                {
                    Client = client,
                    Package = info.Package,
                    CarType = carType,  
                    Route = route,
                    State =  state
                };
                await _db.Orders.AddAsync(order, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                var orderInfo = _mapper.Map<OrderInfo>(order).SetClientData(user.Name, user.Surname, user.PhoneNumber);
                var driverConnectionId = await _driverService.FindDriverConnectionIdAsync(orderInfo, cancellationToken);
                await func(driverConnectionId);
                return new OkObjectResult(info);
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        public async Task<ActionResult> GetWaitingOrdersAsync(string clientUserId,CancellationToken cancellationToken)
        {
            var ordersInfo = new List<OrderInfo>();
            var user = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            var state = await _contextHelper.FindStateAsync((int) GeneralState.Waiting);
            await _contextHelper.Orders(o => o.Client.UserId == clientUserId && o.State == state).
            ForEachAsync(o => ordersInfo.Add(_mapper.Map<OrderInfo>(o)
                    .SetClientData(user.Name, user.Surname, user.PhoneNumber)), cancellationToken);
            return new OkObjectResult(ordersInfo);
        }

        public async Task<ActionResult> GetOnReviewOrdersAsync(string clientUserId, CancellationToken cancellationToken)
        {
            var ordersInfo = new List<OrderInfo>();
            var user = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            var state = await _contextHelper.FindStateAsync((int)GeneralState.OnReview);
            await _contextHelper.Orders(o => o.Client.UserId == clientUserId && o.State == state).
                ForEachAsync(cp => ordersInfo.Add(_mapper.Map<OrderInfo>(cp)
                    .SetClientData(user.Name, user.Surname, user.PhoneNumber)), cancellationToken);
            return new OkObjectResult(ordersInfo);
        }
    }
}