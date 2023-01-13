using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values.Enums;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ChatHubServices
{
    public class ChatHubService : IChatHub
    {
        private readonly IContext _context;

        public ChatHubService(IContext context)
        {
            _context = context;
        }

        public async Task ConnectedAsync(string userId, string connectId)
        {
            var chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == userId)
                          ?? await CreateChatHubAsync(userId, connectId);
            await _context.UpdateAsync(chatHub.UpdateConnectId(connectId));
        }


        public async Task<string> GetConnectionIdAsync(string userId, CancellationToken cancellationToken)
        {
            var chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == userId);
            return chatHub?.ConnectionId;
        }
        public async Task<string> FindDriverConnectionIdAsync(Order order,
            CancellationToken cancellationToken)
        {
            var deliveries = await Deliveries(order, cancellationToken);
            foreach (var delivery in deliveries)
            {
                var chatHub = await _context.FindAsync<ChatHub>(c => c.UserId == delivery.RouteTrip.Driver.UserId);
                if (string.IsNullOrEmpty(chatHub?.ConnectionId)) continue;
                if (await CheckRejectedAsync(delivery.RouteTrip.Id, order.Id)) continue;
                order.State = await _context.FindAsync<State>((int)GeneralState.OnReview);
                await _context.UpdateAsync(delivery.AddOrder(order));
                return chatHub.ConnectionId;
            }
            return string.Empty;
        }
        public async Task DisconnectedAsync(string connectId)
        {
            var chatHub = await _context.FindAsync<ChatHub>(c => c.ConnectionId == connectId);
            await _context.UpdateAsync(chatHub.RemoveConnectId());
        }
        private async Task<ChatHub> CreateChatHubAsync(string userId, string connectId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NullReferenceException();
            }
            var chatHub = new ChatHub(userId, connectId);
            await _context.AddAsync(chatHub);
            return chatHub;
        }
        
        private async Task<List<Delivery>> Deliveries(Order order, CancellationToken cancellationToken) => 
            await _context
                .Deliveries()
                .IncludeRouteTripAndDriverBuilder()
                .Where(r => 
                    r.RouteTrip.Route.StartCity.Id == order.Route.StartCityId && 
                    r.RouteTrip.Route.FinishCity.Id == order.Route.FinishCityId && 
                    r.RouteTrip.DeliveryDate.Day >= order.DeliveryDate.Day)
                .ToListAsync(cancellationToken);
        
        private async Task<bool> CheckRejectedAsync(int routeTripId, int orderId) => 
            await _context
                .AnyAsync<RejectedOrder>(r =>
                    r.RouteTrip.Id == routeTripId &&
                    r.Order.Id == orderId);
    }
}