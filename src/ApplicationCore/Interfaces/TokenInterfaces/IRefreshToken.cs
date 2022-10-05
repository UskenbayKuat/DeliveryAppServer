using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.TokenInterfaces
{
    public interface IRefreshToken
    {
        public Task<string> RefreshTokenAsync(RefreshTokenInfo tokenInfo);
    }
}    