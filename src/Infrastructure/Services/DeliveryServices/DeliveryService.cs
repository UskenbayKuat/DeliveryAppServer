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
        private readonly StateHelper _stateHelper;

        public DeliveryService(AppDbContext db, AppIdentityDbContext identityDbContext, IMapper mapper, StateHelper stateHelper)
        {
            _db = db;
            _identityDbContext = identityDbContext;
            _mapper = mapper;
            _stateHelper = stateHelper;
        }

        public async Task<string> AddToDeliveryAsync(int orderId)
        {
            var order = await _db.Orders.Include(c => c.Client).FirstAsync(c => c.Id == orderId);
            order.State =  _stateHelper.FindState((int)GeneralState.PendingForHandOver);
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return (await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == order.Client.UserId))?.ConnectionId;
        }


        public async Task<ActionResult> GetActiveDeliveriesForClient(string userClientId, CancellationToken cancellationToken)
        {
            var deliveriesInfo = new List<DeliveryInfo>();
            var userClient = await _identityDbContext.Users.FirstAsync(u => u.Id == userClientId, cancellationToken);
            var state =  _stateHelper.FindState((int)GeneralState.InProgress);
            var orders = await OrdersAsync(state, userClient.Id, cancellationToken);
            orders.ForEach( o =>
            {
                var userDriver  = _identityDbContext.Users.First(u => u.Id == o.Delivery.RouteTrip.Driver.UserId);
                deliveriesInfo.Add(_mapper.Map<DeliveryInfo>(o)
                    .SetDriverData(userDriver.Name, userDriver.Surname, userDriver.PhoneNumber)
                    .SetClientData(userClient.Name, userClient.Surname));
            }); 
            
            //ForAll
            
            return new OkObjectResult(deliveriesInfo);
        }
        
        
        private async Task<List<Order>> OrdersAsync(State state, string userClientId, CancellationToken cancellationToken) => 
            await _db.Orders
            .Include(c => c.Delivery.RouteTrip.Driver)
            .Include(c => c.Route.StartCity)
            .Include(c => c.Route.FinishCity)
            .Include(c => c.Delivery)
            .Include(c => c.Package)
            .Include(c => c.State)
            .Where(c => c.Client.UserId == userClientId && c.Delivery.RouteTrip.IsActive && c.State == state).ToListAsync(cancellationToken);
        
    }
}