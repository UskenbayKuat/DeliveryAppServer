using ApplicationCore.Models.Enums;

namespace PublicApi.Commands
{
    public class MobileLoggerCommand
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public MobileEnum MobileEnum { get; set; }
    }
}
