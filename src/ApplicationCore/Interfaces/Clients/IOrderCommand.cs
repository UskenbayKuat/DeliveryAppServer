using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.Clients
{
    public interface IOrderCommand
    {
        public Task<Order> CreateAsync(CreateOrderDto dto, Guid clientUserId);
        public Task<Guid> QRCodeAcceptAsync(QRCodeDto dto);
        public Task<Order> RejectAsync(Guid orderId);
        Task SetDeliveryAsync(Order order, Delivery delivery);
        Task<bool> IsOnReview(BackgroundOrder backgroundOrder);
        Task<Order> UpdateStatePendingAsync(Guid orderId);
        Task CancelAsync(Guid orderId);
        Task<Guid> ProfitAsync(ProfitOrderDto dto);
    }
}