using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.SharedInterfaces;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Services;

namespace PublicApi.Endpoints.Shared.UserData
{
    [Authorize]
    public class UserData : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IUserData _userData;
        private readonly UserService _userService;

        public UserData(IUserData userData, UserService userService)
        {
            _userData = userData;
            _userService = userService;
        }
        
        [HttpPost("api/userData")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await _userData.SendUser(_userService.GetUserId(HttpContext), cancellationToken);
            }        
            catch(NotExistUserException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }        
            catch(HttpRequestException ex) when(ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch(WebException ex) when(ex.Status == WebExceptionStatus.Timeout)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch
            {
                return new BadRequestObjectResult("User not found");
            }
        }
    }
}