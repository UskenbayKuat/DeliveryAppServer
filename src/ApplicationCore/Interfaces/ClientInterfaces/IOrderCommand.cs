using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IOrderCommand
    {
        public Task<Order> CreateAsync(OrderInfo info, string clientUserId, CancellationToken cancellationToken);
        public Task<ActionResult> ConfirmHandOverAsync(ConfirmHandOverInfo info, CancellationToken cancellationToken);
        public Task<Order> RejectAsync(int orderId);
    }
}