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

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDeliveryCommand
    {
        public  Task<Delivery> FindIsNewDelivery(Order order);
        public Task<Delivery> CreateAsync(CreateDeliveryDto dto);
        Task<IReadOnlyList<Order>> AddWaitingOrdersAsync(Delivery delivery);
        public Task<ActionResult> CancellationAsync(string driverUserId);
        public Task StartAsync(string driverUserId);
        Task<LocationDto> UpdateLocationAsync(LocationDto request);
    }
}