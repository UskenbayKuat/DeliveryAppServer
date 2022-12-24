using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IDriver
    {       

        public Task<ActionResult> GetOnReviewOrdersForDriverAsync(string userDriverId);
        public Task<ActionResult> GetActiveOrdersForDriverAsync(string userDriverId);
        public Task<ActionResult> RejectNextFindDriverAsync(string driverUserId, OrderInfo orderInfo,Func<string, Task> func);
        public Task<string> FindDriverConnectionIdAsync(Order order, CancellationToken cancellationToken);
    }
}