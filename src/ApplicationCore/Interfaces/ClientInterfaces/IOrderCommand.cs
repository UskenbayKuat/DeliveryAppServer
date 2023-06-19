using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Entities.Orders;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IOrderCommand
    {
        public Task<Order> CreateAsync(CreateOrderDto dto, string clientUserId, CancellationToken cancellationToken);
        public Task<ActionResult> ConfirmHandOverAsync(ConfirmHandOverInfo info, CancellationToken cancellationToken);
        public Task<Order> RejectAsync(int orderId);
        Task SetDeliveryAsync(Order order, Delivery delivery);
        Task<bool> IsOnReview(BackgroundOrder backgroundOrder);
    }
}