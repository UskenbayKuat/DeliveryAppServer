using ApplicationCore.Enums;

namespace ApplicationCore.Entities.AppEntities
{
    public class Car : BaseEntity
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public Color Color { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
        public int CarTypeId { get; set; }
        public CarType CarType { get; set; }
        public Driver Driver { get; set; }
        public bool IsDeleted { get; set; }
    }
}