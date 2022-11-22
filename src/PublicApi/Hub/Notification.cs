using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Interfaces.OrderInterfaces;
using Infrastructure.Config.Attributes;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace PublicApi.Hub
{
    [Authorize]
    public class Notification : Hub<IHubMethod>
    {
        private readonly AppDbContext _db;
        private readonly IHubConnect _hubConnect;
        private readonly IOrder _order;

        public Notification(AppDbContext db, IHubConnect hubConnect, IOrder order)
        {
            _db = db;
            _hubConnect = hubConnect;
            _order = order;
        }
        public async Task ReceiveDriverInfo(Location location) =>
            await Clients.All.SendClientInfoToDriver(await _order.FindClientPackagesAsync(Context.GetHttpContext().Items["UserId"]?.ToString()));


        public override async Task<Task> OnConnectedAsync()
        {
            await _hubConnect.ConnectedUser(Context.GetHttpContext().Items["UserId"]?.ToString(), Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            await _hubConnect.DisconnectedUser(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}