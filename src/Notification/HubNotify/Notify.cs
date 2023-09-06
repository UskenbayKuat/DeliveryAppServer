using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Models.Dtos.Shared;
using Microsoft.AspNetCore.SignalR;
using Notification.Interfaces;
using Notification.Hubs;

namespace Notification.HubNotify
{
    public class Notify : INotify
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IChatHub _chatHub;

        public Notify(IHubContext<NotificationHub> hubContext, IChatHub chatHub)
        {
            _hubContext = hubContext;
            _chatHub = chatHub;
        }

        public async Task SendToDriverAsync(string userId, CancellationToken cancellationToken)
        {
            var connectionDriverId = await _chatHub.GetConnectionIdAsync(userId, cancellationToken);
            if (!string.IsNullOrEmpty(connectionDriverId))
            {
                await _hubContext.Clients
                    .Client(connectionDriverId ?? string.Empty)
                    .SendCoreAsync("SendToDriver", new[] { "У вас новый заказ" }, cancellationToken);
            }
        }

        public async Task SendToClient(string userId, CancellationToken cancellationToken)
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

        public async Task SendDriverLocationToClientsAsync(string driverUserId, LocationDto locationCommand)
        {
            var connectionIdList = await _chatHub.GetConnectionIdListAsync(driverUserId);
            foreach (var connectionId in connectionIdList)
            {
                await _hubContext.Clients
                    .Client(connectionId ?? string.Empty)
                    .SendCoreAsync("ReceiveDriverLocation", new[] { locationCommand });
            }
        }

        public async Task SendInfoToClientsAsync(string driverUserId)
        {
            var connectionIdList = await _chatHub.GetConnectionIdListAsync(driverUserId);
            foreach (var connectionId in connectionIdList)
            {
                await _hubContext.Clients
                    .Client(connectionId ?? string.Empty)
                    .SendCoreAsync("SendToClient", new[] { "Водитель начал поездку" });
            }
        }

        public async Task SendProfitClientAsync(string clientUserId)
        {
            var connectionId = await _chatHub.GetConnectionIdAsync(clientUserId, default);
            await _hubContext.Clients
                .Client(connectionId ?? string.Empty)
                .SendCoreAsync("SendToClient", new[] { "Водитель прибыль" });
        }

        public async Task QrCodeClientAsync(string clientUserId)
        {
            var connectionId = await _chatHub.GetConnectionIdAsync(clientUserId, default);
            await _hubContext.Clients
                .Client(connectionId ?? string.Empty)
                .SendCoreAsync("SendToClient", new[] { "" });
        }
    }
}