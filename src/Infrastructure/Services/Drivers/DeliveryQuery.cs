using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Drivers;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Specifications.Deliveries;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Drivers
{
    public class DeliveryQuery : IDeliveryQuery
    {
        private readonly IAsyncRepository<Delivery> _context;

        public DeliveryQuery(
            IAsyncRepository<Delivery> context)
        {
            _context = context;
        }
        public async Task<ActiveDeliveryDto> GetDeliveryIsActiveAsync(Guid driverUserId)
        {
            var deliverySpec = new DeliveryWithOrderSpecification(driverUserId);
            var delivery = await _context
                .GetQueryableAsync(deliverySpec)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (delivery is null)
            {
                return default;
            }
            var orderDtoList = (
                from order in delivery.Orders
                select order.GetOrderDto(delivery.Driver.User, delivery.State.StateValue)).ToList();
            return delivery.MapToDeliveryDto(orderDtoList);
        }

        public async Task<List<HistoryDeliveryDto>> GetHistoryAsync(Guid userId)
        {
            var deliverySpec = new DeliveryWithOrderSpecification(userId, isActive: false);
            var deliveryList = await _context
                .GetQueryableAsync(deliverySpec)
                .OrderByDescending(x => x.CreatedDate)
                .AsNoTracking()
                .ToListAsync();
            var resultList = new List<HistoryDeliveryDto>();
            foreach (var delivery in deliveryList)
            {
                var orderDtoList = (
                    from order in delivery.Orders
                    select order.GetOrderDto(delivery.Driver.User, delivery.State.StateValue)).ToList();
                resultList.Add(delivery.MapToHistoryDto(orderDtoList));
            }
            return resultList;
        }
    }
}