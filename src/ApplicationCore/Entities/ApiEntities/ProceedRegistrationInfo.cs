namespace ApplicationCore.Entities.ApiEntities
{
    public class ProceedRegistrationInfo
    {
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string IdentityCardFaceScanPath { get; set; }
        public string IdentityCardBackScanPath { get; set; }
        public string DrivingLicenceScanPath { get; set; }
        public string DriverPhoto { get; set; }
        public bool IsDriver { get; set; }
        public int Id { get; set; }
    }
}