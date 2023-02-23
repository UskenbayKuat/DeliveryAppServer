using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientServices
{
    public class OrderQuery : IOrderQuery
    {
        private readonly AppIdentityDbContext _dbIdentityDbContext;
        private readonly IOrderContextBuilder _orderContextBuilder;

        public OrderQuery(IOrderContextBuilder orderContextBuilder, AppIdentityDbContext dbIdentityDbContext)
        {
            _orderContextBuilder = orderContextBuilder;
            _dbIdentityDbContext = dbIdentityDbContext;
        }
        public async Task<ActionResult> GetWaitingOrdersAsync(string clientUserId, CancellationToken cancellationToken)
        {
            var ordersInfo = new List<OrderInfo>();
            var userClient = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(u => u.Id == clientUserId, cancellationToken);
            await _orderContextBuilder.OrdersInfoBuilder()
                .Build()
                .Where(o => o.Client.UserId == clientUserId && 
                            (o.State.Id == (int)GeneralState.Waiting || 
                             o.State.Id == (int)GeneralState.OnReview))
                .ForEachAsync(o => ordersInfo.Add(o.SetOrderInfo(userClient)), cancellationToken);
            return new OkObjectResult(ordersInfo);
        }
        
        public async Task<ActionResult> GetActiveOrdersForClientAsync(string userClientId)
        {
            var userClient = await _dbIdentityDbContext.Users.FirstAsync(u => u.Id == userClientId);
            return new OkObjectResult(await DeliveriesInfoAsync(userClient));
        }
        
        private async Task<List<DeliveryInfo>> DeliveriesInfoAsync(User userClient)
        {
            var deliveriesInfo = new List<DeliveryInfo>();
            await _orderContextBuilder
                .DeliveriesInfoBuilder()
                .Build()
                .Where(o =>
                    o.Client.UserId == userClient.Id && (
                        o.Delivery.State.Id == (int)GeneralState.New ||
                        o.Delivery.State.Id == (int)GeneralState.InProgress) && (
                        o.State.Id == (int)GeneralState.InProgress ||
                        o.State.Id == (int)GeneralState.PendingForHandOver ||
                        o.State.Id == (int)GeneralState.ReceivedByDriver))
                .ForEachAsync(o =>
                {
                    var userDriver = _dbIdentityDbContext.Users.First(u => u.Id == o.Delivery.Driver.UserId);
                    var deliveryInfo = o.SetDeliveryInfo(userClient, userDriver);
                    deliveriesInfo.Add(deliveryInfo);
                });
            return deliveriesInfo;
        }
    }
}