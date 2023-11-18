using System;

namespace PublicApi.Commands.Deliveries.Models
{
    public class CreateCarCommand
    {
        public Guid CarBrandId { get; set; }
        public Guid CarTypeId { get; set; }
        public Guid CarColorId { get; set; }
        public int ProductionYear { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
    }
}