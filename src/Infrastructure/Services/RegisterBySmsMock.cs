using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using AutoMapper;
using Infrastructure.Config;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class RegisterBySmsMock : IRegistration
    {
        private readonly IMapper _mapper;
        private readonly IGenerateTokenService _generateTokenService;
        private readonly AppIdentityDbContext _identityDb;
        private readonly AppDbContext _db;

        public RegisterBySmsMock(IMapper mapper, AppIdentityDbContext identityDb, AppDbContext db, IGenerateTokenService generateTokenService)
        {
            _mapper = mapper;
            _identityDb = identityDb;
            _db = db;
            _generateTokenService = generateTokenService;
        }

        public async Task<ActionResult> SendTokenAsync(RegistrationToken token) => 
            await RandomSms(token);

        private Task<ActionResult> RandomSms(RegistrationToken token) =>
            Task.FromResult<ActionResult>(new OkObjectResult(token));
        
        
        public async Task<ActionResult> Confirm(ConfirmRegistrationToken info, CancellationToken cancellationToken)
        {
            var user = await _identityDb.Users.FirstOrDefaultAsync(u => u.PhoneNumber == info.PhoneNumber && u.IsDriver == info.IsDriver, cancellationToken);
            if (user != null)
            {
                if (user.IsDriver)
                {
                    var driver = await _db.Drivers.FirstOrDefaultAsync(d => d.UserId == user.Id, cancellationToken);
                    if (driver != null)
                    {
                        return new OkObjectResult(new {driver.IsValid, driver.UserId, driver.Id, user.Name, user.Surname, user.PhoneNumber});
                    }
                }
                else
                {
                    var client = await _db.Clients.FirstOrDefaultAsync(c => c.UserId == user.Id, cancellationToken);
                    if (client != null)
                    {
                        return new OkObjectResult(new {client.IsValid, client.UserId, client.Id, user.Name, user.Surname});
                    }
                }
            }
            user = new User
            {
                PhoneNumber = info.PhoneNumber, 
                IsDriver = info.IsDriver,
                RefreshToken = _generateTokenService.CreateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddYears(AuthOptions.LifeTimeRefreshTokenInYear)
            };
            var userToken = _generateTokenService.CreateAccessToken(user);
            user.Token = userToken;
            await _identityDb.Users.AddAsync(user, cancellationToken);
            await _identityDb.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(new{UserId = user.Id, IsValid = false, user.RefreshToken,  user.Token});
        }
    }
}