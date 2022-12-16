using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IDriver
    {       
        public Task<List<OrderInfo>> FindOrdersAsync(string driverUserId);
        public Task<ActionResult> GetOnReviewOrdersForDriverAsync(string userDriverId);
        public Task<ActionResult> RejectNextFindDriverAsync(string driverUserId, OrderInfo orderInfo,Func<string, OrderInfo, Task> func);
        public Task<string> FindDriverConnectionIdAsync(OrderInfo orderInfo, CancellationToken cancellationToken);
    }
}