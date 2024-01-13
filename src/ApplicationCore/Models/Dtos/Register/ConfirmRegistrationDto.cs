namespace ApplicationCore.Models.Dtos.Register
{
    public class ConfirmRegistrationDto
    {
        public string PhoneNumber { get; set; }
        public string SmsCode { get; set; }
        public bool IsDriver { get; set; }
        public bool IsValid { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}