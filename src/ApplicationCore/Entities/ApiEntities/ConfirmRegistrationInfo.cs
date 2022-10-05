namespace ApplicationCore.Entities.ApiEntities
{
    public class ConfirmRegistrationInfo
    {
        public string PhoneNumber { get; set; }
        public string SmsCode { get; set; }
        public bool IsDriver { get; set; }
        public bool IsValid { get; set; }
        public string UserId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}