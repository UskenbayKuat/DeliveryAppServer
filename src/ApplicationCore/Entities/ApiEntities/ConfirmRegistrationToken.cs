namespace ApplicationCore.Entities.ApiEntities
{
    public class ConfirmRegistrationToken
    {
        public string PhoneNumber { get; set; }
        public string SmsCode { get; set; }
        public bool IsDriver { get; set; }
    }
}