using System.ComponentModel.DataAnnotations;

namespace PublicApi.Commands.Register
{
    public class ConfirmRegisterCommand
    {
        [Required]
        public string SmsCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDriver { get; set; }
    }
}