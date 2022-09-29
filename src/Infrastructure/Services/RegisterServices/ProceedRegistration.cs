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
        private readonly IMapper _mapper;
        
        public ProceedRegistrationService(AppDbContext db, IMapper mapper, AppIdentityDbContext identityDb)
        {
            _db = db;
            _mapper = mapper;
            _identityDb = identityDb;
        }

        public async Task<ActionResult> ProceedRegistration(ProceedRegistrationInfo info, CancellationToken cancellationToken)
        {
            var user = _identityDb.Users.FirstOrDefault(u => u.PhoneNumber == info.PhoneNumber && u.IsDriver == info.IsDriver);
            if (user == null)
            {
                return new BadRequestResult();
            }
            user.Name = info.Name;
            user.Surname = info.Surname;
            await _identityDb.SaveChangesAsync(cancellationToken);
            
            if (user.IsDriver)
            {
                var driver = new Driver()
                {
                    UserId = user.Id,
                    IdentityCardBackScanPath = info.IdentityCardBackScanPath,
                    IdentityCardFaceScanPath = info.IdentityCardFaceScanPath,
                    DrivingLicenceScanPath = info.DrivingLicenceScanPath,
                    DriverPhoto = info.DriverPhoto,
                    IsValid = true
                };
                await _db.Drivers.AddAsync(driver, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                info.Id = driver.Id;
            }
            else
            {
                var client = new Client()
                {
                    UserId = user.Id,
                    IsValid = true
                };
                await _db.Clients.AddAsync(client, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                info.Id = client.Id;
            }
            return new OkObjectResult(info);
        }
        
    }
}