using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.AppEntities
{
    public class User : BaseEntity
    {
        public string UserName{ get; private set; }
        public string Surname { get;private set; }
        public string Email { get;private set; }
        public string PhoneNumber { get;private set; }
        public bool IsDriver { get;private set; }
        public bool IsValid { get;private set; }
        public string RefreshToken { get;private set; }
        public DateTime? RefreshTokenExpiryTime { get;private set; }
        public virtual List<Driver> Drivers { get; private set; } = new();
        public virtual List<Client> Clients { get; private set; } = new();
        public User(string userName, string phoneNumber, string email, string surname)
        {
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Surname = surname;
        }
        public User(string phoneNumber, bool isDriver)
        {
            PhoneNumber = phoneNumber;
            IsDriver = isDriver;
        }

        public User AddRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
            return this;
        }
        public User AddFullName(string name, string surname)
        {
            UserName = name;
            Surname = surname;
            IsValid = true;
            return this;
        }
    }
}