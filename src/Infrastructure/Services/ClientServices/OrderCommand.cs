using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.Values;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientServices
{
    public class OrderCommand : IOrderCommand
    {
        private readonly IContext _context;
        private readonly IOrderContextBuilder _orderContextBuilder;

        public OrderCommand(IContext context, IOrderContextBuilder orderContextBuilder)
        {
            _context = context;
            _orderContextBuilder = orderContextBuilder;
        }

        public async Task<Order> CreateAsync(OrderInfo info, string clientUserId, CancellationToken cancellationToken)
        {
            var client = await _context.FindAsync<Client>(c => c.UserId == clientUserId);
            var carType = await _context.FindAsync<CarType>(c => c.Id == info.CarType.Id);
            var route = await _context.FindAsync<Route>(r =>
                r.StartCityId == info.StartCity.Id &&
                r.FinishCityId == info.FinishCity.Id);
            var state = await _context.FindAsync<State>((int)GeneralState.Waiting);
            var order = new Order(info.IsSingle, info.Price, info.DeliveryDate)
            {
                Client = client,
                Package = info.Package,
                CarType = carType,
                Route = route,
                State = state,
                Location = new Location(info.Location.Latitude, info.Location.Longitude)
            };
            await _context.AddAsync(order);
            return order;
        }

        public async Task<ActionResult> ConfirmHandOverAsync(ConfirmHandOverInfo info, CancellationToken cancellationToken)
        {
            var order = await _orderContextBuilder
                .StateBuilder()
                .Build()
                .FirstOrDefaultAsync(o => o.Id == info.OrderId, cancellationToken);
            if (order.State.Id != (int)GeneralState.PendingForHandOver || order.SecretCode != info.SecretCode.ToUpper()) //TODO проверка на deliveryId == order.Delivery.Id нужен?
            {
                return new BadRequestResult();
            }
            order.State = await _context.FindAsync<State>(s => s.Id == (int)GeneralState.ReceivedByDriver);
            await _context.UpdateAsync(order);
            return new NoContentResult();
        }

        public async Task<Order> RejectAsync(int orderId)
        {
            var order = await _orderContextBuilder.ForRejectBuilder()
                .Build()
                .FirstOrDefaultAsync(o => o.Id == orderId);
            await _context.AddAsync(new RejectedOrder
            {
                Order = order,
                Delivery = order.Delivery
            });
            order.Delivery = default;
            order.State = await _context.FindAsync<State>((int)GeneralState.Waiting);
            await _context.UpdateAsync(order);
            return order;
        }
    }
}