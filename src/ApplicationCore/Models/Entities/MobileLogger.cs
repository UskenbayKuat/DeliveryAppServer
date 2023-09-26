using ApplicationCore.Entities;

namespace ApplicationCore.Models.Entities
{
    public class MobileLogger : BaseEntity
    {

        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public MobileLogger(string message, string stackTrace, string fullName, string email, string type)
        {
            Message = message;
            StackTrace = stackTrace;
            FullName = fullName;
            Email = email;
            Type = type;
        }
    }
}
