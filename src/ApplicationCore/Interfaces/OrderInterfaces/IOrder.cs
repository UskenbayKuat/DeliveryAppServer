using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.OrderInterfaces
{
    public interface IOrder
    {
        public Task<List<OrderInfo>> FindClientPackagesAsync(string userId);
        public Task<string> CreateAsync(string driverUserId, int clientPackageId);
        public Task<string> RejectAsync(string userId, OrderInfo orderInfo, CancellationToken cancellationToken);
        public Task<string> FindRouteTripAsync(OrderInfo orderInfo, CancellationToken cancellationToken);
    }
}