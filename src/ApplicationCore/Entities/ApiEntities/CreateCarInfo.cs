using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Enums;

namespace ApplicationCore.Entities.ApiEntities
{
    public class CreateCarInfo
    {
        public int CarBrandId { get; set; }
        public int CarTypeId { get; set; }
        public int CarColorId { get; set; }
        public int ProductionYear { get; set; }
        public string RegistrationCertificate { get; set; }
        public string LicensePlate { get; set; }
        public bool IsDeleted { get; set; }
    }
}