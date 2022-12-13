using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IDriver
    {       
        public Task<List<OrderInfo>> FindClientPackagesAsync(string driverUserId);
        public Task<ActionResult> SendOnReviewOrdersToDriverAsync(string driverUserId);
        public Task<ActionResult> SendRouteTripToDriverAsync(string driverUserId);
        public Task<string> RejectNextFindDriverConnectionIdAsync(string driverUserId, OrderInfo orderInfo, CancellationToken cancellationToken);
        public Task<string> FindDriverConnectionIdAsync(OrderInfo orderInfo, CancellationToken cancellationToken);
    }
}