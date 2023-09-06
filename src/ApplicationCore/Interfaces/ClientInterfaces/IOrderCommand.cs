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

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IOrderCommand
    {
        public Task<Order> CreateAsync(CreateOrderDto dto, string clientUserId);
        public Task<string> QRCodeAcceptAsync(QRCodeDto dto);
        public Task<Order> RejectAsync(int orderId);
        Task SetDeliveryAsync(Order order, Delivery delivery);
        Task<bool> IsOnReview(BackgroundOrder backgroundOrder);
        Task<Order> UpdateStatePendingAsync(int orderId);
        Task CancelAsync(int orderId);
        Task<string> ProfitAsync(ProfitOrderDto dto);
    }
}