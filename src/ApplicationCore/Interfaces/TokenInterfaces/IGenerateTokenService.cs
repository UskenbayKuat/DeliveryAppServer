using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.TokenInterfaces
{
    public interface IGenerateTokenService
    {
        public string CreateRefreshToken();
        public string CreateAccessToken(User user);

    }
}