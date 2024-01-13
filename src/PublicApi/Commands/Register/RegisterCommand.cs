using System.ComponentModel.DataAnnotations;

namespace PublicApi.Commands.Register
{
    public class RegisterCommand
    {
        public string PhoneNumber { get; set; }
        public bool IsDriver { get; set; }
    }
}