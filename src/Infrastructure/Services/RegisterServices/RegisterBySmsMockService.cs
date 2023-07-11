using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using ApplicationCore.Models.Dtos.Register;
using ApplicationCore.Models.Values;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RegisterServices
{
    public class RegisterBySmsMockService : IRegistration
    {
        private readonly IGenerateToken _generateToken;
        private readonly AppIdentityDbContext _identityDb;

        public RegisterBySmsMockService(AppIdentityDbContext identityDb, IGenerateToken generateToken)
        {
            _identityDb = identityDb;
            _generateToken = generateToken;
        }

        public async Task<ActionResult> SendTokenAsync(RegistrationDto dto) => 
            await RandomSms(dto);

        private Task<ActionResult> RandomSms(RegistrationDto dto) =>
            Task.FromResult<ActionResult>(new OkObjectResult(dto));
        
        public async Task<ActionResult> Confirm(ConfirmRegistrationDto dto, CancellationToken cancellationToken)
        {
             var user = await _identityDb.Users.FirstOrDefaultAsync(
                u => u.PhoneNumber == dto.PhoneNumber && u.IsDriver == dto.IsDriver, cancellationToken)
                 ?? await GetUserAsync(dto.PhoneNumber, dto.IsDriver);
            _identityDb.Users.Update(
                user.AddRefreshToken(
                            refreshToken:_generateToken.CreateRefreshToken(), 
                            refreshTokenExpiryTime: DateTime.UtcNow.AddYears(_generateToken.LifeTimeRefreshTokenInYear)));
            await _identityDb.SaveChangesAsync(cancellationToken);

            return new OkObjectResult(new{accessToken = _generateToken.CreateAccessToken(user), 
                refreshToken = user.RefreshToken, name = user.Name, surname = user.Surname, isValid = user.IsValid, email = user.Email});
        }

        private async Task<User> GetUserAsync(string phoneNumber, bool isDriver)
        {
            var user = new User(phoneNumber, isDriver);
            await _identityDb.Users.AddAsync(user);
            await _identityDb.SaveChangesAsync();
            return user;
        }
    }
}