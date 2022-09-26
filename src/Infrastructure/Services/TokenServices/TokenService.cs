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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.TokenServices
{
    public class TokenService : IGenerateTokenService, IRefreshTokenService
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
            var claimList = new List<Claim> { new("PhoneNumber", user.PhoneNumber) };
            return new JwtSecurityTokenHandler().WriteToken(AuthOptions.SecurityToken(claimList, Key));
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> RefreshToken(RefreshTokenInfo tokenInfo)
        {
            var newAccessToken = String.Empty;
            string phoneNumber = GetPhoneNumberFromToken(tokenInfo.AccessToken);
            var user = await _identityDb.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber &&
                                                                        u.RefreshToken == tokenInfo.RefreshToken &&
                                                                        u.RefreshTokenExpiryTime >= DateTime.Now);
            if (user is not null)
            {
                newAccessToken = CreateAccessToken(user);
                user.Token = newAccessToken;
                _identityDb.Users.Update(user);
                await _identityDb.SaveChangesAsync();
            }
            return newAccessToken;
        }

        private string GetPhoneNumberFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, AuthOptions.ValidationParameters(Key, false),
                    out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(x => x.Type == "PhoneNumber").Value;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}