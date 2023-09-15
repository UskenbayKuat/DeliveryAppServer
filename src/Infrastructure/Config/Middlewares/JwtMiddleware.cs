using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Config.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthOptions _options;


        public JwtMiddleware(RequestDelegate next, IOptions<AuthOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer", string.Empty).Trim();
            if (!string.IsNullOrEmpty(token))
                CheckValidate(context, token);
            await _next(context);
        }

        private void CheckValidate(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _options.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _options.Audience,      
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey)),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                
                
                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                context.Items["UserId"] = userId;
            }
            catch
            {
                // TODO: need to add logger
            }
        }
    }
}