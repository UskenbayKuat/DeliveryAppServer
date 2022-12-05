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

        public Task<ActionResult> GetActiveOrdersForClient(string userId, CancellationToken cancellationToken);
        public Task<string> CreateAsync(string driverUserId, int clientPackageId);

    }
}