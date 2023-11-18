using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Orders;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.Drivers
{
    public interface IDeliveryCommand
    {
        public Task<Delivery> FindIsActiveDeliveryAsync(Order order);
        public Task<Delivery> CreateAsync(CreateDeliveryDto dto);
        Task<IReadOnlyList<Order>> AddWaitingOrdersAsync(Delivery delivery);
        public Task<ActionResult> CancellationAsync(Guid driverUserId);
        public Task StartAsync(Guid driverUserId);
        Task<LocationDto> UpdateLocationAsync(LocationDto request);
        Task FinishAsync(Guid userId);
    }
}