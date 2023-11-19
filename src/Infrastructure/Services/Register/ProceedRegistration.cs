using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Register;
using ApplicationCore.Models.Dtos.Register;
using ApplicationCore.Specifications.Users;
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

        public async Task ProceedRegistration(ProceedRegistrationDto dto, Guid userId,
            CancellationToken cancellationToken)
        {
            var userSpec = new UserForProceedSpecification(userId);
            var user = (await _contextUser.FirstOrDefaultAsync(userSpec, cancellationToken))
                       .AddData(dto.UserName, dto.Surname, dto.Email) ??
                       throw new NotExistUserException("User is not found");
            if (user.IsDriver)
            {
                if (user.Driver != null)
                {
                    throw new ArgumentException("Данные уже существует, нельзя изменить");
                }
                user.Driver = new Driver(
                    dto.IdentificationNumber, 
                    dto.IdentificationSeries,
                    dto.IdentityCardCreateDate, 
                    dto.DriverLicenceScanPath, 
                    dto.IdentityCardPhotoPath);
            }
            else
            {
                if (user.Client != null)
                {
                    throw new ArgumentException("Данные уже существует, нельзя изменить");
                }
                user.Client = new();
            }

            await _contextUser.UpdateAsync(user);
        }
    }
}