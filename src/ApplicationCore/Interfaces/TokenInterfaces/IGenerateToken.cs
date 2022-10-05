using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.TokenInterfaces
{
    public interface IGenerateToken
    {
        public string CreateRefreshToken();
        public string CreateAccessToken(User user);

    }
}