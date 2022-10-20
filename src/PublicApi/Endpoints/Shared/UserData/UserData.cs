using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.SharedInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.UserData
{
    [Authorize]
    public class UserData : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IUserData _userData;

        public UserData(IUserData userData)
        {
            _userData = userData;
        }
        
        [HttpPost("api/userData")]
        public override Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.Claims.First(c => c.Type == ClaimTypes.UserData).Value;
                return  _userData.SendUser(userId, cancellationToken);
            }
            catch
            {
                return Task.FromResult<ActionResult>(new BadRequestObjectResult("User not found"));
            }
        }
    }
}