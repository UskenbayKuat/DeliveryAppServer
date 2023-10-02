using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Models.Dtos.Shared;

namespace Notification.Services
{
    public interface INotify
    {
        public Task SendToDriverAsync(string userId, CancellationToken cancellationToken);
        public Task SendToClient(string userId, CancellationToken cancellationToken);
        public Task SendDriverLocationToClientsAsync(string driverUserId, LocationDto locationCommand);
        public Task SendInfoToClientsAsync(string driverUserId);
        public Task SendProfitClientAsync(string clientUserId);
        Task QrCodeClientAsync(string clientUserId);
    }
}