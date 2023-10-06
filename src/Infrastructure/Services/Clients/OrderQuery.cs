using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using ApplicationCore.Specifications.Deliveries;
using ApplicationCore.Specifications.Orders;
using Ardalis.Specification;
using Infrastructure;
using Infrastructure.Config;
using Infrastructure.Context.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Clients
{
    public class OrderQuery : IOrderQuery
    {
        private readonly AppIdentityDbContext _dbIdentityDbContext;
        private readonly IAsyncRepository<Order> _context;
        private readonly IOrderStateHistory _stateHistory;

        public OrderQuery(
            AppIdentityDbContext dbIdentityDbContext,
            IAsyncRepository<Order> context, IOrderStateHistory stateHistory)
        {
            _dbIdentityDbContext = dbIdentityDbContext;
            _context = context;
            _stateHistory = stateHistory;
        }

        public async Task<List<DeliveryDto>> GetActiveOrdersForClientAsync(string clientUserId)
        {
            var orderSpec = new OrderWithStateSpecification(clientUserId);
            return await GetDeliveryDtosAsync(orderSpec, clientUserId);
        }


        public async Task<IReadOnlyList<Order>> GetByDriverUserIdAsync(string driverUserId)
        {
            var orderSpec = new OrderWithClientSpecification(driverUserId);
            return await _context.ListAsync(orderSpec);
        }

        public async Task<List<DeliveryDto>> GetHistoryAsync(string clientUserId)
        {
            var orderSpec = new OrderWithDeliverySpecification(clientUserId);
            return await GetDeliveryDtosAsync(orderSpec, clientUserId);
        }

        public async Task<IReadOnlyList<Order>> GetWaitingOrders(int routeId, DateTime dateTime)
        {
            var orderSpec = new OrderWithStateSpecification(routeId, dateTime);
            return await _context.ListAsync(orderSpec);
        }
        private async Task<List<DeliveryDto>> GetDeliveryDtosAsync(ISpecification<Order> specification, string userId)
        {

            var orders = await _context
                .GetQueryableAsync(specification)
                .AsNoTracking()
                .ToListAsync();
            var userClient = await _dbIdentityDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
            var deliveriesInfo = new List<DeliveryDto>();
            foreach (var order in orders)
            {
                if (order.State.StateValue == GeneralState.CANCALED)
                {
                    deliveriesInfo.Add(order.GetDeliveryDto(userClient));

                }
                else
                {
                    var driverUserId = order.Delivery?.Driver?.UserId;
                    var orderState = await _stateHistory.GetAsync(order.Id);
                    var userDriver = await _dbIdentityDbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == driverUserId);
                    deliveriesInfo.Add(order.GetDeliveryDto(userClient, userDriver, orderState));
                }
            }
            return deliveriesInfo;
        }
        public async Task<LocationDto> GetCurrentLocationAsync(LocationDto dto)
        {
            var orderSpec = new OrderWithDeliverySpecification(dto.OrderId);
            var order = await _context.FirstOrDefaultAsync(orderSpec)
                ?? throw new ArgumentException($"Нет такой заказ по id {dto.OrderId}");
            var driver = await _dbIdentityDbContext.Users.FirstOrDefaultAsync(x => x.Id == order.Delivery.Driver.UserId);
            return new()
            {
                DriverName = driver.Name,
                DriverPhoneNumber = driver.PhoneNumber,
                DriverSurname = driver.Surname,
                Latitude = order.Delivery.Location.Latitude,
                Longitude = order.Delivery.Location.Longitude
            };
        }
    }
}