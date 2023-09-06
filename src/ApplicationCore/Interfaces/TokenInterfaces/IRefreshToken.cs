using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.TokenInterfaces
{
    public interface IRefreshToken
    {
        public Task<string> RefreshTokenAsync(RefreshTokenDto tokenDto);
    }
}    