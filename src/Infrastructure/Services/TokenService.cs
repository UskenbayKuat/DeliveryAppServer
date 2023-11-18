using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Models.Dtos.Shared;
using Infrastructure.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ApplicationCore.Interfaces;

namespace Infrastructure.Services
{
    public class TokenService : IGenerateToken, IRefreshToken
    {
        private readonly IAsyncRepository<User> _context;
        private readonly AuthOptions _options;
        public int LifeTimeRefreshTokenInYear { get; }
        public TokenService(IOptions<AuthOptions> options, IAsyncRepository<User> context)
        {
            _options = options.Value;
            LifeTimeRefreshTokenInYear = _options.LifeTimeRefreshTokenInYear;
            _context = context;
        }

        public string CreateAccessToken(User user) =>
            new JwtSecurityTokenHandler()
                .WriteToken(
                    new JwtSecurityToken(
                        _options.Issuer,
                        _options.Audience,
                        notBefore: DateTime.UtcNow,
                        claims: new List<Claim> { new(JwtRegisteredClaimNames.Sub, user.Id.ToString()) },
                        signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey)),
                            SecurityAlgorithms.HmacSha256),
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_options.LifeTimeTokenInMinute)))
                );

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> RefreshTokenAsync(RefreshTokenDto tokenDto)
        {
            var user = await _context
                .FirstOrDefaultWithAsNoTrackingkAsync(u =>
                        u.RefreshToken == tokenDto.RefreshToken &&
                        u.RefreshTokenExpiryTime >= DateTime.Now)
                ?? throw new ArgumentException("Invalid account");
            return CreateAccessToken(user);
        }
    }
}