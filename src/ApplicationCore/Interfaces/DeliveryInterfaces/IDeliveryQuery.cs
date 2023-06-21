using System.Threading.Tasks;
using ApplicationCore.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDeliveryQuery
    {
        public Task<DeliveryDto> GetDeliveryIsActiveAsync(string driverUserId);
    }
}