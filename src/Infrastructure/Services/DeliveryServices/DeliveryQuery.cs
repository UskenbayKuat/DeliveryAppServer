using System;
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
        public async Task<IsActiveDeliveryDto> GetDeliveryIsActiveAsync(string driverUserId)
        {
            var deliverySpec = new DeliveryWithOrderSpecification(driverUserId);
            var delivery = await _context
                .GetQueryableAsync(deliverySpec)
                .AsNoTracking()
                .FirstOrDefaultAsync() ?? throw new ArgumentException("Не найден поездка");
            var orderDtoList = (
                from order in delivery.Orders 
                let user = _identityDbContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == order.Client.UserId) 
                select order.SetOrderInfo(user)).ToList();
            return delivery.GetDeliveryDto(orderDtoList);
        }
    }
}