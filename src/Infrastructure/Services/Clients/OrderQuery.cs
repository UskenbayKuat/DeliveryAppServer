using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using ApplicationCore.Specifications.Orders;
using Ardalis.Specification;
using Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Clients
{
    public class OrderQuery : IOrderQuery
    {
        private readonly IAsyncRepository<Order> _context;
        private readonly IOrderStateHistory _stateHistory;

        public OrderQuery(
            IAsyncRepository<Order> context,
            IOrderStateHistory stateHistory)
        {
            _context = context;
            _stateHistory = stateHistory;
        }

        public async Task<List<DeliveryDto>> GetActiveOrdersForClientAsync(Guid clientUserId)
        {
            var orderSpec = new OrderWithStateSpecification(clientUserId, false);
            return await GetDeliveryDtosAsync(orderSpec, clientUserId);
        }


        public async Task<IReadOnlyList<Order>> GetByDriverUserIdAsync(Guid driverUserId)
        {
            var orderSpec = new OrderWithClientSpecification(driverUserId, true);
            return await _context.ListAsync(orderSpec);
        }

        public async Task<List<DeliveryDto>> GetHistoryAsync(Guid clientUserId)
        {
            var orderSpec = new OrderWithDeliverySpecification(clientUserId, true);
            return await GetDeliveryDtosAsync(orderSpec, clientUserId);
        }

        public async Task<IReadOnlyList<Order>> GetWaitingOrders(Guid routeId, DateTime dateTime)
        {
            var orderSpec = new OrderWithStateSpecification(routeId, dateTime);
            return await _context.ListAsync(orderSpec);
        }
        private async Task<List<DeliveryDto>> GetDeliveryDtosAsync(ISpecification<Order> specification, Guid userId)
        {
            var orders = await _context
                .GetQueryableAsync(specification)
                .AsNoTracking()
                .ToListAsync();
            var deliveriesInfo = new List<DeliveryDto>();
            foreach (var order in orders)
            {
                if (order.State.StateValue == GeneralState.CANCALED)
                {
                    order.Delivery.Driver = null;
                    deliveriesInfo.Add(order.GetDeliveryDto());
                }
                else
                {
                    var orderState = await _stateHistory.GetAsync(order.Id);
                    deliveriesInfo.Add(order.GetDeliveryDto(orderState));
                }
            }
            return deliveriesInfo;
        }
        public async Task<LocationDto> GetCurrentLocationAsync(LocationDto dto)
        {
            var orderSpec = new OrderWithDeliverySpecification(dto.OrderId);
            var order = await _context.FirstOrDefaultAsync(orderSpec)
                ?? throw new ArgumentException($"Нет такой заказ по id {dto.OrderId}");
            return new()
            {
                DriverName = order.Delivery.Driver.User.UserName,
                DriverPhoneNumber = order.Delivery.Driver.User.PhoneNumber,
                DriverSurname = order.Delivery.Driver.User.Surname,
                Latitude = order.Delivery.Location.Latitude,
                Longitude = order.Delivery.Location.Longitude
            };
        }
    }
}