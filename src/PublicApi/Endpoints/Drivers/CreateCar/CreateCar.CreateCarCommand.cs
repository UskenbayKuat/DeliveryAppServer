using ApplicationCore.Enums;

namespace PublicApi.Endpoints.Drivers.CreateCar
{
    public class CreateCarCommand
    {
        public int DriverId { get; set; }
        public int CarBrandId { get; set; }
        public int CarTypeId { get; set; }
        public int CarColorId { get; set; }
        public int ProductionYear { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
        public bool IsDeleted { get; set; }
    }
}