using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Specifications.Deliveries;
using Infrastructure.AppData.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DeliveryServices
{
    public class DeliveryQuery : IDeliveryQuery
    {
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IAsyncRepository<Delivery> _context;

        public DeliveryQuery(AppIdentityDbContext identityDbContext, IAsyncRepository<Delivery> context)
        {
            _identityDbContext = identityDbContext;
            _context = context;
        }
        public async Task<ActiveDeliveryDto> GetDeliveryIsActiveAsync(string driverUserId)
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
                let user = _identityDbContext.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Id == order.Client.UserId) 
                select order.GetOrderDto(user, delivery.State.StateValue)).ToList();
            return delivery.MapToDeliveryDto(orderDtoList);
        }

        public async Task<List<HistoryDeliveryDto>> GetHistoryAsync(string userId)
        {
            var deliverySpec = new DeliveryWithOrderSpecification(userId, isActive: false);
            var deliveryList = await _context
                .GetQueryableAsync(deliverySpec)
                .AsNoTracking()
                .ToListAsync();
            var resultList = new List<HistoryDeliveryDto>();
            foreach (var delivery in deliveryList)
            {
                var orderDtoList = (
                    from order in delivery.Orders
                    let user = _identityDbContext.Users
                        .AsNoTracking()
                        .FirstOrDefault(u => u.Id == order.Client.UserId)
                    select order.GetOrderDto(user, delivery.State.StateValue)).ToList();
                resultList.Add(delivery.MapToHistoryDto(orderDtoList));
            }
            return resultList;
        }
    }
}