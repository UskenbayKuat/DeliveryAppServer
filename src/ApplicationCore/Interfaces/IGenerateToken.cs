using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities;
using System;

namespace ApplicationCore.Interfaces
{
    public interface IGenerateToken
    {
        public string CreateRefreshToken();
        public string CreateAccessToken(User user);
        public DateTime GetLifeTimeRefreshToken();

    }
}