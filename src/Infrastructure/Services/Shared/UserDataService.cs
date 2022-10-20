using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
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

        public Task<ActionResult> SendUser(string userId, CancellationToken cancellationToken)
        {
            var user = _dbIdentity.Users.First(u => u.Id == userId);
            if (user.IsDriver)
            {
                var driver = _db.Drivers.Include(d => d.Car).First(d => d.UserId == userId);
                return Task.FromResult<ActionResult>(new ObjectResult(driver));
            }
            
            var client = _db.Clients.First(c => c.UserId == userId);
            return Task.FromResult<ActionResult>(new OkObjectResult(client));
        }
    }
}