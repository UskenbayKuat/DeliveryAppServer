using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDeliveryCommand
    {
        public  Task<Delivery> FindIsNewDelivery(Order order, CancellationToken cancellationToken);
        public Task<Delivery> CreateAsync(RouteTripInfo tripInfo, string userId);
        public Task<Order> AddOrderAsync(int orderId);


    }
}