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

        public async Task<string> RefreshTokenAsync(RefreshTokenInfo tokenInfo)
        {
    //        var phoneNumber = GetPhoneNumberFromToken(tokenInfo.AccessToken);
            var user = _identityDb.Users.First(u => u.RefreshToken == tokenInfo.RefreshToken &&
                                                    u.RefreshTokenExpiryTime >= DateTime.Now);
            user.Token = CreateAccessToken(user);
            _identityDb.Users.Update(user);
            await _identityDb.SaveChangesAsync();
            return user.Token;
        }
        
        //check accessToken
        // private string GetPhoneNumberFromToken(string token)
        // {
        //     try
        //     {
        //         var tokenHandler = new JwtSecurityTokenHandler();
        //         tokenHandler.ValidateToken(token, AuthOptions.ValidationParameters(Key, false),
        //             out SecurityToken validatedToken);
        //         var jwtToken = (JwtSecurityToken)validatedToken;
        //         return jwtToken.Claims.First(x => x.Type == "PhoneNumber").Value;
        //     }
        //     catch (Exception)
        //     {
        //         return string.Empty;
        //     }
        // }
    }
}