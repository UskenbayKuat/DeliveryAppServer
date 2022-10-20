using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Enums;

namespace ApplicationCore.Entities.AppEntities
{
    public class Car : BaseEntity
    {
        public int DriverId { get; set; }
        public int CarBrandId { get; set; } //TODO add CarBrand
        public int CarTypeId { get; set; } //TODO add CarType
        public int CarColorId { get; set; } //TODO add CarColor
        public int ProductionYear { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
        public bool IsDeleted { get; set; }
    }
}