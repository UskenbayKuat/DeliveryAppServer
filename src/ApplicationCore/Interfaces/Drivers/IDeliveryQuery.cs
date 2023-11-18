using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Deliveries;

namespace ApplicationCore.Interfaces.Drivers
{
    public interface IDeliveryQuery
    {
        public Task<ActiveDeliveryDto> GetDeliveryIsActiveAsync(Guid driverUserId);
        Task<List<HistoryDeliveryDto>> GetHistoryAsync(Guid userId);
    }
}