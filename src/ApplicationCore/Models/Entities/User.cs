using System;

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
        public Driver Driver { get; set; } 
        public Client Client { get; set; } 
        public User(string userName, string phoneNumber, string email, string surname, bool isValid)
        {
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Surname = surname;
            IsValid = isValid;
        }
        public User()
        {
            
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
        public User AddData(string name, string surname, string email)
        {
            UserName = name;
            Surname = surname;
            Email = email;
            IsValid = true;
            return this;
        }
    }
}