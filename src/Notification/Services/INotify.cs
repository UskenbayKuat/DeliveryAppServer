using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Shared;

namespace Notification.Services
{
    public interface INotify
    {
        public Task SendToDriverAsync(Guid userId, CancellationToken cancellationToken);
        public Task SendToClient(Guid userId, CancellationToken cancellationToken);
        public Task SendDriverLocationToClientsAsync(Guid driverUserId, LocationDto locationCommand);
        public Task SendInfoToClientsAsync(Guid driverUserId);
        public Task SendProfitClientAsync(Guid clientUserId);
        Task QrCodeClientAsync(Guid clientUserId);
    }
}