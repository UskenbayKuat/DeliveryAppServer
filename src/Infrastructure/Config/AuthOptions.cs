using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Config
{
    public static class AuthOptions
    {
        private const int LifeTimeTokenInMinute = 20;
        public const int LifeTimeRefreshTokenInYear = 1;
        private const string Issuer = "DeliveryApp";
        private const string Audience = "AuthClient";


        public static TokenValidationParameters ValidationParameters(byte[] key, bool validateLifetime)
        {
            return  new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,      
                ValidateLifetime = validateLifetime,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        public static JwtSecurityToken SecurityToken(List<Claim> claims, byte[] key)
        {
            return new JwtSecurityToken(
                Issuer,
                Audience,
                notBefore: DateTime.UtcNow,
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256),
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(LifeTimeTokenInMinute)));
        }
    }
}