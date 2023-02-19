using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.HubInterfaces;
using Microsoft.AspNetCore.SignalR;
using Notification.Interfaces;

namespace Notification.HubNotify
{
    public class Notify : INotify
    {
        private readonly IHubContext<Notification> _hubContext;
        private readonly IChatHub _chatHub;

        public Notify(IHubContext<Notification> hubContext, IChatHub chatHub)
        {
            _hubContext = hubContext;
            _chatHub = chatHub;
        }

        public async Task SendToDriverAsync(string userId, CancellationToken cancellationToken)
        {
            var connectionDriverId = await _chatHub.GetConnectionIdAsync(userId, cancellationToken);
            if (!string.IsNullOrEmpty(connectionDriverId))
            {
                await _hubContext.Clients.Client(connectionDriverId)
                    .SendCoreAsync("SendToDriver", new[] { "У вас новый заказ" }, cancellationToken);
            }
        }

        public async Task SendToClient(string userId, CancellationToken cancellationToken)
        {
            var connectionClientId = await _chatHub.GetConnectionIdAsync(userId, cancellationToken);
            if (!string.IsNullOrEmpty(connectionClientId))
            {
                await _hubContext.Clients.Client(connectionClientId).SendCoreAsync("SendToClient",
                    new[] { "Ваш заказ принят, ожидает передачи" }, cancellationToken);
            }
        }

        public async Task SendDriverLocationToClientsAsync(List<Order> orders, LocationInfo locationCommand)
        {
            var connectionIdList = await _chatHub.GetConnectionIdListAsync(orders);
            foreach (var connectionId in connectionIdList)
            {
                await _hubContext.Clients.Client(connectionId).SendCoreAsync("ReceiveDriverLocation", new[] { locationCommand });
            }
        }
    }
}