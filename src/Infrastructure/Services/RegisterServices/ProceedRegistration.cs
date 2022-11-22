using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.RegisterInterfaces;
using AutoMapper;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RegisterServices
{
    public class ProceedRegistrationService : IProceedRegistration
    {
        private readonly AppDbContext _db;
        private readonly AppIdentityDbContext _identityDb;

        public ProceedRegistrationService(AppDbContext db, AppIdentityDbContext identityDb)
        {
            _db = db;
            _identityDb = identityDb;
        }

        public async Task<ActionResult> ProceedRegistration(ProceedRegistrationInfo info, string userId, CancellationToken cancellationToken)
        {
            try
            {
                var user = (await _identityDb.Users.FirstAsync(u => u.Id == userId, cancellationToken)).AddFullName(info.Name, info.Surname);
                if (user.IsDriver)
                {
                    var driver = new Driver(user.Id, info.IdentificationNumber, info.IdentificationSeries,
                        info.IdentityCardCreateDate, info.DriverLicenceScanPath, info.IdentityCardPhotoPath);
                    await _db.Drivers.AddAsync(driver, cancellationToken);
                }
                else
                {
                    await _db.Clients.AddAsync(new Client(user.Id), cancellationToken);
                }
                _identityDb.Users.Update(user);
                await _identityDb.SaveChangesAsync(cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                return new OkObjectResult(new {name = user.Name, surname = user.Surname});
            }
            catch
            {
                return new BadRequestObjectResult("User is not found");
            }
        }
        
    }
}