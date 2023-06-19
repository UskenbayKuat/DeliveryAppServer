using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Models.Entities.Orders;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDeliveryCommand
    {
        public  Task<Delivery> FindIsNewDelivery(Order order);
        public Task<Delivery> CreateAsync(RouteTripInfo tripInfo, string userId);
        public Task<Order> AddOrderAsync(int orderId);
        public Task<ActionResult> CancellationAsync(string driverUserId);
        public Task StartAsync(string driverUserId);
        
    }
}