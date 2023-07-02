using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values;
using ApplicationCore.Specifications.Orders;
using Infrastructure.AppData.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ClientServices
{
    public class OrderQuery : IOrderQuery
    {
        private readonly AppIdentityDbContext _dbIdentityDbContext;
        private readonly IAsyncRepository<Order> _context;

        public OrderQuery(
            AppIdentityDbContext dbIdentityDbContext, 
            IAsyncRepository<Order> context)
        {
            _dbIdentityDbContext = dbIdentityDbContext;
            _context = context;
        }
        
        public async Task<List<DeliveryDto>> GetActiveOrdersForClientAsync(string clientUserId)
        {
            var userClient = await _dbIdentityDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == clientUserId);
            var deliveriesInfo = new List<DeliveryDto>();
            var orderSpec = new OrderWithStateSpecification(clientUserId);
            await _context
                .GetQueryableAsync(orderSpec)
                .AsNoTracking()
                .ForEachAsync(o =>
                {
                    var driverUserId = o.Delivery?.Driver?.UserId;
                    var userDriver = _dbIdentityDbContext.Users
                        .AsNoTracking()
                        .FirstOrDefault(u => u.Id == driverUserId);
                    deliveriesInfo.Add(o.GetDeliveryDto(userClient, userDriver));
                });
            return deliveriesInfo;
        } 
        

        public async Task<IReadOnlyList<Order>> GetByDriverUserIdAsync(string driverUserId)
        {
            var orderSpec = new OrderWithClientSpecification(driverUserId);
            return await _context.ListAsync(orderSpec);
        }

        public async Task<IReadOnlyList<Order>> GetWaitingOrders(int routeId, DateTime dateTime)
        {
            var orderSpec = new OrderWithStateSpecification(routeId, dateTime);
            return await _context.ListAsync(orderSpec);
        }
    }
}