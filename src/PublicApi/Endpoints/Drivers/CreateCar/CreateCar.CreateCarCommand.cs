using ApplicationCore.Enums;

namespace PublicApi.Endpoints.Drivers.CreateCar
{
    public class CreateCarCommand
    {
        public string CarBrand { get; set; }
        public string CarType { get; set; }
        public string CarColor { get; set; }
        public int ProductionYear { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
        public bool IsDeleted { get; set; }
    }
}