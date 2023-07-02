using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Models.Dtos.Register;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RegisterServices
{
    public class ProceedRegistrationService : IProceedRegistration
    {
        private readonly AppIdentityDbContext _identityDb;
        private readonly IAsyncRepository<Driver> _contextDriver;
        private readonly IAsyncRepository<Client> _contextClient;
        public ProceedRegistrationService(AppIdentityDbContext identityDb, IAsyncRepository<Driver> contextDriver, IAsyncRepository<Client> contextClient)
        {
            _identityDb = identityDb;
            _contextDriver = contextDriver;
            _contextClient = contextClient;
        }

        public async Task<ActionResult> ProceedRegistration(ProceedRegistrationDto dto, string userId,
            CancellationToken cancellationToken)
        {
            var user = (await _identityDb.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken))
                       .AddFullName(dto.Name, dto.Surname) ??
                       throw new NotExistUserException("User is not found");
            if (user.IsDriver)
            {
                var driver = new Driver(user.Id, dto.IdentificationNumber, dto.IdentificationSeries,
                    dto.IdentityCardCreateDate, dto.DriverLicenceScanPath, dto.IdentityCardPhotoPath);
                await _contextDriver.AddAsync(driver, cancellationToken);
            }
            else
            {
                await _contextClient.AddAsync(new Client(user.Id), cancellationToken);
            }

            _identityDb.Users.Update(user);
            await _identityDb.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(new { name = user.Name, surname = user.Surname });
        }
    }
}