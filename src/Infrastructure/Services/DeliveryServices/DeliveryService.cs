using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DeliveryServices
{
    public class DeliveryService : IDelivery
    {
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IContext _context;

        public DeliveryService(AppIdentityDbContext identityDbContext, IContext context)
        {
            _identityDbContext = identityDbContext;
            _context = context;
        }

        public async Task<ActionResult> AddToDeliveryAsync(int orderId, Func<string, Task> func)
        {
            var order = await _context.Orders().IncludeClientBuilder().FirstAsync(c => c.Id == orderId);
            order.State = await _context.FindAsync<State>((int)GeneralState.PendingForHandOver);
            await _context.UpdateAsync(order);
            await func((await _context.FindAsync<ChatHub>(c => c.UserId == order.Client.UserId))?.ConnectionId);
            return new NoContentResult();
        }


        public async Task<ActionResult> GetActiveOrdersForClientAsync(string userClientId)
        {
            var deliveriesInfo = new List<DeliveryInfo>();
            var userClient = await _identityDbContext.Users.FirstAsync(u => u.Id == userClientId);
            var stateInProgress =  await _context.FindAsync<State>((int)GeneralState.InProgress);
            var stateHandOver = await _context.FindAsync<State>((int)GeneralState.PendingForHandOver);
            var stateReceived = await _context.FindAsync<State>((int)GeneralState.ReceivedByDriver);
            var order2 = _context.GetAll<Order>();
            var we = await order2.Include(o => o.State).Include(o => o.Client).ToListAsync();
            var orders = await _context 
                .Orders()
                .IncludeDeliveriesInfoBuilder()
                .Where(c =>
                    c.Client.UserId == userClientId &&
                    c.Delivery.RouteTrip.IsActive &&
                    c.State == stateInProgress || c.State == stateHandOver || c.State == stateReceived).ToListAsync();
            foreach (var order in orders)
            {
                var userDriver = await _identityDbContext.Users.FirstAsync(u => u.Id == order.Delivery.RouteTrip.Driver.UserId);
                var deliveryInfo = order.GetDeliveryInfo(userClient, userDriver);
                var location = await _context.GetAll<LocationDate>()
                    .Include(r => r.Location)
                    .FirstOrDefaultAsync(l => l.RouteTrip.Id == order.Delivery.RouteTrip.Id); // maybe create builder for LocationDateBuilder
                deliveryInfo.Location = location?.Location;
                deliveriesInfo.Add(deliveryInfo);
            }

            return new OkObjectResult(deliveriesInfo);
        }
    }
}