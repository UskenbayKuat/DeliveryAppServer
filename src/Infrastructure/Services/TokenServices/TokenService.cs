using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces.TokenInterfaces;
using Infrastructure.Config;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.TokenServices
{
    public class TokenService : IGenerateToken, IRefreshToken
    {
        private readonly AppIdentityDbContext _identityDb;
        private byte[] Key { get; }

        public TokenService(IConfiguration config, AppIdentityDbContext identityDb)
        {
            _identityDb = identityDb;
            Key = Encoding.ASCII.GetBytes(config.GetSection("JwtSettings:SecretKey").Value);
        }

        public string CreateAccessToken(User user)
        {
            var claimList = new List<Claim> { new(ClaimTypes.UserData, user.Id) };
            return new JwtSecurityTokenHandler()
                .WriteToken(AuthOptions.SecurityToken(claimList, Key));
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public Task<ActionResult> RefreshTokenAsync(RefreshTokenInfo tokenInfo)
        {
            var user = _identityDb.Users.First(u => u.RefreshToken == tokenInfo.RefreshToken &&
                                                    u.RefreshTokenExpiryTime >= DateTime.Now);
            return Task.FromResult<ActionResult>(new OkObjectResult(CreateAccessToken(user)));
        }
    }
}