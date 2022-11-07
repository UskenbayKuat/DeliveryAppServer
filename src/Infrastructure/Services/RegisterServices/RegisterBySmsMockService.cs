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
        private readonly IGenerateToken _generateToken;
        private readonly AppIdentityDbContext _identityDb;
        private readonly AppDbContext _db;

        public RegisterBySmsMockService(AppIdentityDbContext identityDb, AppDbContext db, IGenerateToken generateToken)
        {
            _identityDb = identityDb;
            _db = db;
            _generateToken = generateToken;
        }

        public async Task<ActionResult> SendTokenAsync(RegistrationInfo info) => 
            await RandomSms(info);

        private Task<ActionResult> RandomSms(RegistrationInfo info) =>
            Task.FromResult<ActionResult>(new OkObjectResult(info));
        
        public async Task<ActionResult> Confirm(ConfirmRegistrationInfo info, CancellationToken cancellationToken)
        {
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
                        info.IsValid = user.IsValid;
                    }
                }
                else
                {
                    var client = await _db.Clients.FirstOrDefaultAsync(c => c.UserId == user.Id, cancellationToken);
                    if (client != null)
                    {
                        info.IsValid = user.IsValid;
                    }
                }
                info.Name = user.Name;
                info.Surname = user.Surname;
            }
            user.RefreshToken = _generateToken.CreateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddYears(_generateToken.LifeTimeRefreshTokenInYear);
            _identityDb.Users.Update(user);
            await _identityDb.SaveChangesAsync(cancellationToken);

            info.AccessToken = _generateToken.CreateAccessToken(user);
            info.RefreshToken = user.RefreshToken;
            return new OkObjectResult(info);
        }
    }
}