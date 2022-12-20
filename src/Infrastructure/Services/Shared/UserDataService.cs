using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Shared
{
    public class UserDataService : IUserData
    {
        private readonly AppIdentityDbContext _dbIdentity;
        private readonly AppDbContext _db;

        public UserDataService(AppDbContext db, AppIdentityDbContext dbIdentity)
        {
            _db = db;
            _dbIdentity = dbIdentity;
        }

        public async Task<ActionResult> GetDataAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _dbIdentity.Users.FirstAsync(u => u.Id == userId, cancellationToken);
            if (user.IsDriver)
            {
                var driver = await _db.Drivers.Include(d => d.Car)
                                        .FirstAsync(d => d.UserId == userId, cancellationToken);
                return new ObjectResult(driver);
            }
            
            var client = await _db.Clients.FirstAsync(c => c.UserId == userId, cancellationToken);
            return new OkObjectResult(client);
        }
    }
}