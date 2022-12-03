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
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Interfaces.OrderInterfaces;
using Infrastructure.AppData.DataAccess;
using Infrastructure.Config.Attributes;
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
        private readonly IDriver _driverService;

        public Notification(AppDbContext db, IHubConnect hubConnect, IDriver driverService)
        {
            _db = db;
            _hubConnect = hubConnect;
            _driverService = driverService;
        }
        public async Task ReceiveDriverInfo(Location location) =>
            await Clients.All.SendClientInfoToDriver(await _driverService.FindClientPackagesAsync(Context.GetHttpContext().Items["UserId"]?.ToString()));


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