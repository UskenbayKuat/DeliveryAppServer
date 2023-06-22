using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Specifications.Deliveries;
using Infrastructure.AppData.Identity;

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
        public async Task<DeliveryDto> GetDeliveryIsActiveAsync(string driverUserId)
        {
            var deliverySpec = new DeliveryWithOrderSpecification(driverUserId);
            var delivery = await _context.FirstOrDefaultAsync(deliverySpec);
            var orderDtoList = (
                from order in delivery.Orders 
                let user = _identityDbContext.Users.FirstOrDefault(u => u.Id == order.Client.UserId) 
                select order.SetOrderInfo(user)).ToList();
            return delivery.GetDeliveryDto(orderDtoList);
        }
    }
}