namespace PublicApi.Endpoints.RegisterApi.ProceedRegisterDriver
{
    public class ProceedRegisterCommand
    {
        public string PhoneNumber { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string IdentityCardFaceScanPath { get; set; } = null!;
        public string IdentityCardBackScanPath { get; set; } = null!;
        public string DrivingLicenceScanPath { get; set; }
        public string DriverPhoto { get; set; }
        public bool IsDriver { get; set; }
    }
}