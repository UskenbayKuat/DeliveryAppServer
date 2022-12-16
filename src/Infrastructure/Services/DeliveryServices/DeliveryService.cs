using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using AutoMapper;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DeliveryServices
{
    public class DeliveryService : IDelivery
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IMapper _mapper;
        private readonly ContextHelper _contextHelper;

        public DeliveryService(AppDbContext db, AppIdentityDbContext identityDbContext, IMapper mapper,
            ContextHelper contextHelper)
        {
            _db = db;
            _identityDbContext = identityDbContext;
            _mapper = mapper;
            _contextHelper = contextHelper;
        }

        public async Task<ActionResult> AddToDeliveryAsync(int orderId, Func<string, Task> func)
        {
            var order = await _db.Orders.Include(c => c.Client).FirstAsync(c => c.Id == orderId);
            order.State = await _contextHelper.FindStateAsync((int)GeneralState.PendingForHandOver);
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            await func((await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == order.Client.UserId))?.ConnectionId);
            return new OkObjectResult(new OrderInfo());
        }


        public async Task<ActionResult> GetActiveOrdersForClientAsync(string userClientId)
        {
            var deliveriesInfo = new List<DeliveryInfo>();
            var userClient = await _identityDbContext.Users.FirstAsync(u => u.Id == userClientId);
            var stateInProgress = await _contextHelper.FindStateAsync((int)GeneralState.InProgress);
            var stateHandOver = await _contextHelper.FindStateAsync((int)GeneralState.PendingForHandOver);
            var stateReceived = await _contextHelper.FindStateAsync((int)GeneralState.ReceivedByDriver);
            await _contextHelper
                .Orders(c =>
                    c.Client.UserId == userClientId &&
                    c.Delivery.RouteTrip.IsActive &&
                    c.State == stateInProgress || c.State == stateHandOver || c.State == stateReceived)
                .ForEachAsync(o =>
                {
                    var userDriver = _identityDbContext.Users.First(u => u.Id == o.Delivery.RouteTrip.Driver.UserId);
                    deliveriesInfo.Add(_mapper.Map<DeliveryInfo>(o)
                        .SetDriverData(userDriver.Name, userDriver.Surname, userDriver.PhoneNumber)
                        .SetClientData(userClient.Name, userClient.Surname));
                });
            return new OkObjectResult(deliveriesInfo);
        }
    }
}