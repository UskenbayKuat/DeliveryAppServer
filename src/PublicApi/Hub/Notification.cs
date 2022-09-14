using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace PublicApi.Hub
{
    public class Notification : Microsoft.AspNetCore.SignalR.Hub
    {
        private int count = 0;
        private readonly AppDbContext _db;

        public Notification(AppDbContext db)
        {
            _db = db;
        }

        public async Task Confirm(string hubId)
        {
            await Clients.All.SendAsync("Receive", $"Пришло {count}");

        }
        public async Task ReceiveDriverInfo(string hubId, string routeTripId, string latitude, string longitude)
        {
            var routeId = Convert.ToInt32(routeTripId);
            var l1 = Convert.ToDouble(latitude, System.Globalization.CultureInfo.InvariantCulture);
            var l2 = Convert.ToDouble(longitude, System.Globalization.CultureInfo.InvariantCulture);
            Location location = new()
            {
                Latitude = l1,
                Longitude = l2
            };
            var routeTrip = await _db.RouteTrips.FirstOrDefaultAsync(r => r.Id == routeId);

            _db.Locations.Add(location);
            
            routeTrip.HubId = hubId;
            routeTrip.LocationId = location.Id;
            routeTrip.Location = location;
            _db.RouteTrips.Update(routeTrip);
            await _db.SaveChangesAsync();
            Console.WriteLine($"{hubId}");
        }     
        public async Task ReceiveClientInfo(string hubId, string clientPackageId, string latitude, string longitude)
        {
            count = new Random().Next(0, 30);
            var cpId = Convert.ToInt32(clientPackageId);
            var clientPackage = await _db.ClientPackages.FirstOrDefaultAsync(c => c.Id == cpId);
            var l1 = Convert.ToDouble(latitude, System.Globalization.CultureInfo.InvariantCulture);
            var l2 = Convert.ToDouble(longitude, System.Globalization.CultureInfo.InvariantCulture);
            Location location = new()
            {
                Latitude = l1,
                Longitude = l2
            };
            _db.Locations.Add(location);
            
            clientPackage.HubId = hubId;
            clientPackage.LocationId = location.Id;
            clientPackage.Location = location;
            _db.ClientPackages.Update(clientPackage);
            await _db.SaveChangesAsync();
            var routeTrip = await _db.RouteTrips
                .Where(r => r.TripTime > clientPackage.DateTime)
                .FirstOrDefaultAsync(r => r.StartCityId == clientPackage.StartCityId && r.FinishCityId == clientPackage.FinishCityId);
            var options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            var jsonClientPackage = JsonSerializer.Serialize(clientPackage, options);
            await Clients.All.SendAsync("SendClientPackage", $"Пришло {count}");
        }
        public async Task UpdateLocation(string hubId, string driverId, string latitude, string longitude)
        {
            var routeTrip = await _db.RouteTrips.FirstOrDefaultAsync(r => r.DriverId == int.Parse(driverId));
            if (routeTrip.HubId != hubId)
            {
                routeTrip.HubId = hubId;
                _db.RouteTrips.Update(routeTrip);
            }
            var location = await _db.Locations.FirstOrDefaultAsync(l => l.Id == routeTrip.LocationId);
            if (Math.Abs(location.Latitude - double.Parse(latitude)) > 0.001 && Math.Abs(location.Longitude - double.Parse(longitude)) > 0.001)
            {
                location.Latitude = double.Parse(latitude);
                location.Longitude = double.Parse(longitude);
                _db.Locations.Update(location);
            }
            await _db.SaveChangesAsync();
        }
    }
}