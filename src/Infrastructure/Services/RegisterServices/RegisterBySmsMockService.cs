using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using AutoMapper;
using Infrastructure.Config;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.RegisterServices
{
    public class RegisterBySmsMockService : IRegistration
    {
        private readonly IMapper _mapper;
        private readonly IGenerateToken _generateToken;
        private readonly AppIdentityDbContext _identityDb;
        private readonly AppDbContext _db;

        public RegisterBySmsMockService(IMapper mapper, AppIdentityDbContext identityDb, AppDbContext db, IGenerateToken generateToken)
        {
            _mapper = mapper;
            _identityDb = identityDb;
            _db = db;
            _generateToken = generateToken;
        }

        public async Task<ActionResult> SendTokenAsync(RegistrationToken token) => 
            await RandomSms(token);

        private Task<ActionResult> RandomSms(RegistrationToken token) =>
            Task.FromResult<ActionResult>(new OkObjectResult(token));
        
        public async Task<ConfirmRegistrationInfo> Confirm(ConfirmRegistrationInfo info, CancellationToken cancellationToken)
        {
             bool checkIsValid = false;
            int checkId = default;
            var user = await _identityDb.Users.FirstOrDefaultAsync(
                u => u.PhoneNumber == info.PhoneNumber && u.IsDriver == info.IsDriver, cancellationToken);
            if (user is null)
            {
                user = new User
                {
                    PhoneNumber = info.PhoneNumber,
                    IsDriver = info.IsDriver,
                };
                await _identityDb.Users.AddAsync(user, cancellationToken);
                await _identityDb.SaveChangesAsync(cancellationToken);
            }
            else
            {
                if (user.IsDriver)
                {
                    var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.UserId == user.Id, cancellationToken);
                    if (driver != null)
                    {
                        checkId = driver.Id;
                        checkIsValid = driver.IsValid;
                    }
                }
                else
                {
                    var client = await _db.Clients.FirstOrDefaultAsync(c => c.UserId == user.Id, cancellationToken);
                    if (client != null)
                    {
                        checkId = client.Id;
                        checkIsValid = client.IsValid;
                    }
                }
            }
            user.RefreshToken = _generateToken.CreateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddYears(AuthOptions.LifeTimeRefreshTokenInYear);
            user.Token = _generateToken.CreateAccessToken(user);
            _identityDb.Users.Update(user);
            await _identityDb.SaveChangesAsync(cancellationToken);
            info.Name = user.Name;
            info.Surname = user.Surname;
            info.UserId = user.Id;
            info.IsValid = checkIsValid;
            info.Id = checkId;
            info.AccessToken = user.Token;
            info.RefreshToken = user.RefreshToken;
            return info;
        }
    }
}