using ApplicationCore.Enums;

namespace PublicApi.Endpoints.Drivers.CreateCar
{
    public class CreateCarCommand
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public Color Color { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
        public int CarTypeId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}