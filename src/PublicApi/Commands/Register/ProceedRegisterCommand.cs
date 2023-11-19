using System;

namespace PublicApi.Commands.Register
{
    public class ProceedRegisterCommand
    {
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationSeries { get; set; }
        public DateTime IdentityCardCreateDate { get; set; }
        public string DriverLicenceScanPath { get; set; }
        public string IdentityCardPhotoPath { get; set; }
        public bool IsDriver { get; set; }
    }
}