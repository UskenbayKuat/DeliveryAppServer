using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsDriver { get; set; }
    }
}