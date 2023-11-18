using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Register;
using ApplicationCore.Models.Dtos.Register;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.Register
{
    public class ProceedRegistrationService : IProceedRegistration
    {
        private readonly IAsyncRepository<User> _contextUser;
        public ProceedRegistrationService(IAsyncRepository<User> contextUser)
        {
            _contextUser = contextUser;
        }

        public async Task<ActionResult> ProceedRegistration(ProceedRegistrationDto dto, Guid userId,
            CancellationToken cancellationToken)
        {
            var user = (await _contextUser.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken))
                       .AddFullName(dto.Name, dto.Surname) ??
                       throw new NotExistUserException("User is not found");
            if (user.IsDriver)
            {
                var driver = new Driver(dto.IdentificationNumber, dto.IdentificationSeries,
                    dto.IdentityCardCreateDate, dto.DriverLicenceScanPath, dto.IdentityCardPhotoPath);
                user.Drivers.Add(driver);
            }
            else
            {
                user.Clients.Add(new());
            }

            user = await _contextUser.UpdateAsync(user);
            return new OkObjectResult(new { name = user.UserName, surname = user.Surname, email = user.Email });
        }
    }
}