using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Register;
using ApplicationCore.Models.Dtos.Register;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.Register
{
    public class RegisterBySmsMockService : IRegistration
    {
        private readonly IGenerateToken _generateToken;
        private readonly IAsyncRepository<User> _userRepository;
        public RegisterBySmsMockService(IGenerateToken generateToken, IAsyncRepository<User> userRepository)
        {
            _generateToken = generateToken;
            _userRepository = userRepository;
        }

        public async Task<ActionResult> SendTokenAsync(RegistrationDto dto) => 
            await RandomSms(dto);

        private Task<ActionResult> RandomSms(RegistrationDto dto) =>
            Task.FromResult<ActionResult>(new OkObjectResult(dto));
        
        public async Task<ActionResult> Confirm(ConfirmRegistrationDto dto, CancellationToken cancellationToken)
        {
             var user = await _userRepository.FirstOrDefaultAsync(
                u => u.PhoneNumber == dto.PhoneNumber && u.IsDriver == dto.IsDriver, cancellationToken)
                 ?? await GetUserAsync(dto.PhoneNumber, dto.IsDriver);
            var date = DateTime.UtcNow.AddYears(_generateToken.LifeTimeRefreshTokenInYear);
            await _userRepository.UpdateAsync(
                user.AddRefreshToken(_generateToken.CreateRefreshToken(), date));

            return new OkObjectResult(new{accessToken = _generateToken.CreateAccessToken(user), 
                refreshToken = user.RefreshToken, name = user.UserName, surname = user.Surname, isValid = user.IsValid, email = user.Email});
        }

        private async Task<User> GetUserAsync(string phoneNumber, bool isDriver)
        {
            return await _userRepository.AddAsync(new User(phoneNumber, isDriver));
        }
    }
}