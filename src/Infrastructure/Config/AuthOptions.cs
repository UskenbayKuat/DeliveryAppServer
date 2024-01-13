using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Config
{
    public class AuthOptions
    {
        public const string JwtSettings = "JwtSettings";
        public const string SmsOptions = "SmsOptions";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifeTimeTokenInMinute { get; set; }
        public int LifeTimeRefreshTokenInYear { get; set; }
        public string SecretKey { get; set; }
    }
    public class SmsOptions
    {
        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Query { get; set; }
        public bool IsProd { get; set; }

        public string GetQuery(string phone, string message)
        {
            return string.Format(Query, Url, Login, Password, phone, message);
        }
    }
}