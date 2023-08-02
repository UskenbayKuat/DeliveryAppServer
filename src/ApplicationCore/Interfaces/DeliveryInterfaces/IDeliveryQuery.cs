using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Deliveries;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DeliveryInterfaces
{
    public interface IDeliveryQuery
    {
        public Task<ActiveDeliveryDto> GetDeliveryIsActiveAsync(string driverUserId);
        Task<List<HistoryDeliveryDto>> GetHistoryAsync(string userId);
    }
}