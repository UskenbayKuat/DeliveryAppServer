using System.Linq;
using System.Security.Claims;
using ApplicationCore.Exceptions;
using Microsoft.AspNetCore.Http;

namespace PublicApi.Services
{
    public class UserService
    {
        public string GetUserId(HttpContext httpContext)
        {
            var claimsIdentity = httpContext.User.Identity as ClaimsIdentity ?? 
                                 throw new NotExistUserException("User not exist");
            return  claimsIdentity.Claims.First(c => c.Type == ClaimTypes.UserData).Value;
        }
    }
}