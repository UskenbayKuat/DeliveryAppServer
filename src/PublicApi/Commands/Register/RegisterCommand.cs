using System.ComponentModel.DataAnnotations;

namespace PublicApi.Commands.Register
{
    public class RegisterCommand
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}