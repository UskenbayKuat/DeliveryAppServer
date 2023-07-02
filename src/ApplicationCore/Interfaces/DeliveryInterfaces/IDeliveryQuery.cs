using System.Threading.Tasks;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Deliveries;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDeliveryQuery
    {
        public Task<IsActiveDeliveryDto> GetDeliveryIsActiveAsync(string driverUserId);
    }
}