using System;

namespace ApplicationCore.Models.Dtos
{
    public class CreateCarDto
    {
        public Guid CarBrandId { get; set; }
        public Guid CarTypeId { get; set; }
        public Guid CarColorId { get; set; }
        public int ProductionYear { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
        public Guid UserId { get; private set; }

        public CreateCarDto SetUserId(string userId)
        {
            UserId = Guid.Parse(userId);
            return this;
        }
    }
}