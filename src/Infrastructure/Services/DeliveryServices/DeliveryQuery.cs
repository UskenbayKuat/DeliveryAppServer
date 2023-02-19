using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    public class DeliveryQuery : IDeliveryQuery
    {
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IOrderContextBuilder _orderContextBuilder;
        private readonly IDeliveryContextBuilder _deliveryContextBuilder;

        public DeliveryQuery(AppIdentityDbContext identityDbContext, IOrderContextBuilder orderContextBuilder, IDeliveryContextBuilder deliveryContextBuilder)
        {
            _identityDbContext = identityDbContext;
            _orderContextBuilder = orderContextBuilder;
            _deliveryContextBuilder = deliveryContextBuilder;
        }
        public async Task<ActionResult> GetDeliveryIsActiveAsync(string driverUserId)
        {
            var delivery = await _deliveryContextBuilder
                .RouteBuilder()
                .Build()
                .FirstOrDefaultAsync(d =>
                    d.Driver.UserId == driverUserId &&
                    (d.State.Id == (int)GeneralState.New ||
                     d.State.Id == (int)GeneralState.InProgress));
            return new OkObjectResult(delivery?.SetRouteTripInfo());
        }

        public async Task<ActionResult> GetOnReviewOrdersForDriverAsync(string driverUserId) => 
            await GetOrderInfosAsync(driverUserId, OnReviewOrderExpression());
        public async Task<ActionResult> GetActiveOrdersForDriverAsync(string driverUserId) =>
            await GetOrderInfosAsync(driverUserId, ActiveOrderExpression());


        private async Task<ActionResult> GetOrderInfosAsync(string driverUserId, Expression<Func<Order, bool>> expression)
        {
            var ordersInfo = new List<OrderInfo>();
            await _orderContextBuilder
                .OrdersInfoBuilder()
                .Build()
                .Where(o => o.Delivery.Driver.UserId == driverUserId)
                .Where(expression)
                .ForEachAsync(o =>
                {
                    var userClient = _identityDbContext.Users.FirstOrDefault(u => u.Id == o.Client.UserId);
                    ordersInfo.Add(o.SetSecretCodeEmpty().SetOrderInfo(userClient));
                });
            return new OkObjectResult(ordersInfo);
        }
        
        private Expression<Func<Order, bool>> ActiveOrderExpression() =>
            o => o.State.Id == (int)GeneralState.InProgress ||
                 o.State.Id == (int)GeneralState.PendingForHandOver ||
                 o.State.Id == (int)GeneralState.ReceivedByDriver;
        private Expression<Func<Order, bool>> OnReviewOrderExpression() => 
            o => o.State.Id == (int)GeneralState.OnReview;
    }
}