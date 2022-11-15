using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.HubInterfaces;
using Infrastructure.Config.Attributes;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace PublicApi.Hub
{
    [Authorize]
    public class Notification : Hub<IHub>
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _dbIdentity;

        public Notification(AppDbContext db, AppIdentityDbContext dbIdentity)
        {
            _db = db;
            _dbIdentity = dbIdentity;
        }
        
        public override async Task<Task> OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Items["UserId"]?.ToString() 
                         ?? throw new HubException();
            var chatHub = await _db.ChatHubs.FirstOrDefaultAsync(c => c.UserId == userId);
            if (chatHub is not null)
            {
                chatHub.ConnectionId = Context.ConnectionId;
                _db.ChatHubs.Update(chatHub);
            }
            else
            {
                await _db.ChatHubs.AddAsync(new ChatHub(userId, Context.ConnectionId));
            }
            await _db.SaveChangesAsync();
            return base.OnConnectedAsync();
        }
        
        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var chatHub = await _db.ChatHubs.FirstAsync(c => c.ConnectionId == Context.ConnectionId);
                chatHub.ConnectionId = string.Empty;
                _db.ChatHubs.Update(chatHub);
                await _db.SaveChangesAsync();
                return base.OnDisconnectedAsync(exception);
            }
            catch
            {
                throw new HubException();
            }
        }

        public async Task ReceiveDriverInfo(string routeTripId, string latitude, string longitude)
        {
            var userId2 = Context.Items["UserId"]?.ToString();
            var connectionId = Context.ConnectionId;
            
            //send message for user
            await Clients.All.SendClientInfoToDriver("Complete_123");
        }
        
        
        //InProgress
        #region SendInfo for Client and Driver(create service) 

        // public async Task ReceiveDriverInfo(string routeTripId, string latitude, string longitude)
        // {
        //     var userId = Context.Items["UserId"]?.ToString();
        //     var userId2 = (string)Context.Items["UserId"];
        //     var routeTrip = await _db.RouteTrips.FirstOrDefaultAsync(r => r.Id == Convert.ToInt32(routeTripId));
        //     var location = new Location()
        //     {
        //         Latitude = Convert.ToDouble(latitude, System.Globalization.CultureInfo.InvariantCulture),
        //         Longitude = Convert.ToDouble(longitude, System.Globalization.CultureInfo.InvariantCulture)
        //     };
        //     
        //     var locationDate = new LocationDate
        //     {
        //         Location = location,
        //         RouteTrip = routeTrip,
        //         LocationDateTime = DateTime.Now
        //     };
        //     routeTrip.HubId = hubId;
        //     _db.Locations.Add(location);
        //     _db.LocationDate.Add(locationDate);
        //     _db.RouteTrips.Update(routeTrip);
        //     await _db.SaveChangesAsync();
        // }     
        /*public async Task ReceiveClientInfo(string hubId, string clientPackageId, string latitude, string longitude)
        {
            var clientPackage = await _db.ClientPackages.FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(clientPackageId));
            Location location = new()
            {
                Latitude = Convert.ToDouble(latitude, System.Globalization.CultureInfo.InvariantCulture),
                Longitude = Convert.ToDouble(longitude, System.Globalization.CultureInfo.InvariantCulture)
            };
            _db.Locations.Add(location);
            
            clientPackage.HubId = hubId;
            clientPackage.Location = location;
            _db.ClientPackages.Update(clientPackage);
            await _db.SaveChangesAsync();
            // var routeTrip = await _db.RouteTrips
            //     .Where(r => r.TripTime > clientPackage.DateTime)
            //     .Include(r => r.StartCity)
            //     .Include(r => r.FinishCity)
            //     .FirstOrDefaultAsync(r => r.StartCity == clientPackage.StartCity && r.FinishCity == clientPackage.FinishCity);
            var options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            var jsonClientPackage = JsonSerializer.Serialize(clientPackage, options);
        }*/

        // public async Task UpdateLocation(string hubId, string driverId, string latitude, string longitude)
        // {
        //     var driver = await _db.Drivers.FirstOrDefaultAsync(r => r.Id == int.Parse(driverId));
        //     var routeTrip = await _db.RouteTrips.FirstOrDefaultAsync(r => r.Driver == driver);
        //     if (routeTrip.HubId != hubId)
        //     {
        //         routeTrip.HubId = hubId;
        //         _db.RouteTrips.Update(routeTrip);
        //     }
        //
        //     var location = await _db.Locations.FirstOrDefaultAsync(l => l.Id == routeTrip.Location.Id);
        //     if (Math.Abs(location.Latitude - double.Parse(latitude)) > 0.001 && Math.Abs(location.Longitude - double.Parse(longitude)) > 0.001)
        //     {
        //         location.Latitude = double.Parse(latitude);
        //         location.Longitude = double.Parse(longitude);
        //         _db.Locations.Update(location);
        //     }
        //     await _db.SaveChangesAsync();
        // }

        #endregion
    }
}