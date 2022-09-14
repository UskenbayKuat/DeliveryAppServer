using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class RegisterBySmsMock : IRegistration
    {
        private readonly IMapper _mapper;
        private readonly AppIdentityDbContext _identityDb;
        private readonly AppDbContext _db;

        public RegisterBySmsMock(IMapper mapper, AppIdentityDbContext identityDb, AppDbContext db)
        {
            _mapper = mapper;
            _identityDb = identityDb;
            _db = db;
        }

        public async Task<ActionResult> SendTokenAsync(RegistrationToken token) => 
            await RandomSms(token);

        private Task<ActionResult> RandomSms(RegistrationToken token) =>
            Task.FromResult<ActionResult>(new OkObjectResult(token));
        
        
        public async Task<ActionResult> Confirm(ConfirmRegistrationToken token, CancellationToken cancellationToken)
        {
            var user = await _identityDb.Users.FirstOrDefaultAsync(u => u.PhoneNumber == token.PhoneNumber && u.IsDriver == token.IsDriver, cancellationToken);
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
            user = new User { PhoneNumber = token.PhoneNumber, IsDriver = token.IsDriver};
            await _identityDb.Users.AddAsync(user, cancellationToken);
            await _identityDb.SaveChangesAsync(cancellationToken);
            return new OkObjectResult(new{UserId = user.Id, IsValid = false});
        }
    }
}