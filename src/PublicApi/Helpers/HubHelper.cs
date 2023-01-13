using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PublicApi.HubNotification;

namespace PublicApi.Helpers
{
    public class HubHelper
    {
        private readonly IHubContext<Notification> _hubContext;

        public HubHelper(IHubContext<Notification> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToDriverAsync(string connectionId, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId)
                    .SendCoreAsync("SendToDriver", new[] { "У вас новый заказ" }, cancellationToken);
            }
        }
        public async Task SendToClient(string connectionId, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).
                    SendCoreAsync("SendToClient", new[] { "Ваш заказ принят, ожидает передачи" }, cancellationToken);
            }
        }
    }
}