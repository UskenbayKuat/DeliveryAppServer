using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.RegisterInterfaces;
using AutoMapper;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<ActionResult> ProceedRegistration(ProceedRegistrationInfo info, CancellationToken cancellationToken)
        {
            var user = _identityDb.Users.FirstOrDefault(u => u.PhoneNumber == info.PhoneNumber && u.IsDriver == info.IsDriver);
            if (user is null)
            {
                return new BadRequestResult();
            }
            user.Name = info.Name;
            user.Surname = info.Surname;
            user.IsValid = true;
            await _identityDb.SaveChangesAsync(cancellationToken);
            
            if (user.IsDriver)
            {
                var driver = new Driver
                {
                    UserId = user.Id,
                    IdentificationNumber = info.IdentificationNumber,
                    IdentificationSeries = info.IdentificationSeries,
                    IdentityCardPhotoPath = info.IdentityCardPhotoPath,
                    DriverLicenceScanPath = info.DriverLicenceScanPath,
                    IdentityCardCreateDate = info.IdentityCardCreateDate
                };
                await _db.Drivers.AddAsync(driver, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
            }
            else
            {
                var client = new Client
                {
                    UserId = user.Id,
                };
                await _db.Clients.AddAsync(client, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
            }
            return new OkObjectResult(new {name = user.Name, surname = user.Surname});
        }
        
    }
}