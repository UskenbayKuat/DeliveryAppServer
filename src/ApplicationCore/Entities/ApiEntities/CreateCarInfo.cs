using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Enums;

namespace ApplicationCore.Entities.ApiEntities
{
    public class CreateCarInfo
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