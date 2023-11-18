using ApplicationCore.Entities;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Models.Entities
{
    public class MobileLogger : BaseEntity
    {
        public string Message { get; private set; }
        public string StackTrace { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Type { get; private set; }
        public MobileEnum MobileEnum { get; private set; }
        public MobileLogger(string message, string stackTrace, string fullName, string email, string type, MobileEnum mobileEnum)
        {
            Message = message;
            StackTrace = stackTrace;
            FullName = fullName;
            Email = email;
            Type = type;
            MobileEnum = mobileEnum;
        }
    }
}
