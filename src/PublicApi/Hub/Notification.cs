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
        private readonly AppDbContext _db;

        public Notification(AppDbContext db)
        {
            _db = db;
        }
        
        public async Task ReceiveDriverInfo(string hubId, string routeTripId, string latitude, string longitude)
        {
            var routeTrip = await _db.RouteTrips.FirstOrDefaultAsync(r => r.Id == Convert.ToInt32(routeTripId));
            var location = new Location()
            {
                Latitude = Convert.ToDouble(latitude, System.Globalization.CultureInfo.InvariantCulture),
                Longitude = Convert.ToDouble(longitude, System.Globalization.CultureInfo.InvariantCulture)
            };

            var locationDate = new LocationDate
            {
                Location = location,
                RouteTrip = routeTrip,
                LocationDateTime = DateTime.Now
            };
            routeTrip.HubId = hubId;
            _db.Locations.Add(location);
            _db.LocationDate.Add(locationDate);
            _db.RouteTrips.Update(routeTrip);
            await _db.SaveChangesAsync();
        }     
        public async Task ReceiveClientInfo(string hubId, string clientPackageId, string latitude, string longitude)
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
            await Clients.All.SendAsync("SendClientPackage", $"Пришло ");
        }
        public async Task UpdateLocation(string hubId, string driverId, string latitude, string longitude)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(r => r.Id == int.Parse(driverId));
            var routeTrip = await _db.RouteTrips.FirstOrDefaultAsync(r => r.Driver == driver);
            if (routeTrip.HubId != hubId)
            {
                routeTrip.HubId = hubId;
                _db.RouteTrips.Update(routeTrip);
            }
            // var location = await _db.Locations.FirstOrDefaultAsync(l => l.Id == routeTrip.Location.Id);
            // if (Math.Abs(location.Latitude - double.Parse(latitude)) > 0.001 && Math.Abs(location.Longitude - double.Parse(longitude)) > 0.001)
            // {
            //     location.Latitude = double.Parse(latitude);
            //     location.Longitude = double.Parse(longitude);
            //     _db.Locations.Update(location);
            // }
            await _db.SaveChangesAsync();
        }
    }
}