namespace PublicApi.Commands.Deliveries.Models
{
    public class CreateCarCommand
    {
        public int CarBrandId { get; set; }
        public int CarTypeId { get; set; }
        public int CarColorId { get; set; }
        public int ProductionYear { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
    }
}