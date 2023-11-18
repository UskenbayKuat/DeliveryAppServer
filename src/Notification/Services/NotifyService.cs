using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Shared;
using Microsoft.AspNetCore.SignalR;
using Notification.Hubs;
using ApplicationCore.Interfaces.Shared;
using System;

namespace Notification.Services
{
    public class NotifyService : INotify
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IChatHub _chatHub;

        public NotifyService(IHubContext<NotificationHub> hubContext, IChatHub chatHub)
        {
            _hubContext = hubContext;
            _chatHub = chatHub;
        }

        public async Task SendToDriverAsync(Guid userId, CancellationToken cancellationToken)
        {
            var connectionDriverId = await _chatHub.GetConnectionIdAsync(userId, cancellationToken);
            if (!string.IsNullOrEmpty(connectionDriverId))
            {
                await _hubContext.Clients
                    .Client(connectionDriverId ?? string.Empty)
                    .SendCoreAsync("SendToDriver", new[] { "У вас новый заказ" }, cancellationToken);
            }
        }

        public async Task SendToClient(Guid userId, CancellationToken cancellationToken)
        {
            var connectionClientId = await _chatHub.GetConnectionIdAsync(userId, cancellationToken);
            if (!string.IsNullOrEmpty(connectionClientId))
            {
                await _hubContext.Clients
                    .Client(connectionClientId ?? string.Empty)
                    .SendCoreAsync("SendToClient",
                    new[] { "Ваш заказ принят, ожидает передачи" }, cancellationToken);
            }
        }

        public async Task SendDriverLocationToClientsAsync(Guid driverUserId, LocationDto locationCommand)
        {
            var connectionIdList = await _chatHub.GetConnectionIdListAsync(driverUserId);
            foreach (var connectionId in connectionIdList)
            {
                await _hubContext.Clients
                    .Client(connectionId ?? string.Empty)
                    .SendCoreAsync("ReceiveDriverLocation", new[] { locationCommand });
            }
        }

        public async Task SendInfoToClientsAsync(Guid driverUserId)
        {
            var connectionIdList = await _chatHub.GetConnectionIdListAsync(driverUserId);
            foreach (var connectionId in connectionIdList)
            {
                await _hubContext.Clients
                    .Client(connectionId ?? string.Empty)
                    .SendCoreAsync("SendToClient", new[] { "Водитель начал поездку" });
            }
        }

        public async Task SendProfitClientAsync(Guid clientUserId)
        {
            var connectionId = await _chatHub.GetConnectionIdAsync(clientUserId, default);
            await _hubContext.Clients
                .Client(connectionId ?? string.Empty)
                .SendCoreAsync("SendToClient", new[] { "Водитель прибыль" });
        }

        public async Task QrCodeClientAsync(Guid clientUserId)
        {
            var connectionId = await _chatHub.GetConnectionIdAsync(clientUserId, default);
            await _hubContext.Clients
                .Client(connectionId ?? string.Empty)
                .SendCoreAsync("SendToClient", new[] { "" });
        }
    }
}