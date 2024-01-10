using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;

namespace ApplicationCore.Models.Entities.Cars
{
    public class Car : BaseEntity
    {
        public Car() { }
        public Car(int productionYear, string registrationCertificate, string carNumber, CarBrand carBrand, CarType carType, CarColor carColor)
        {
            ProductionYear = productionYear;
            RegistrationCertificate = registrationCertificate;
            CarNumber = carNumber;
            CarBrand = carBrand;
            CarType = carType;
            CarColor = carColor;
        }
        public int ProductionYear { get; private set; }
        public string RegistrationCertificate { get; private set; }
        public string CarNumber { get; private set; }
        public CarBrand CarBrand { get; set; }
        public CarType CarType { get; set; }
        public CarColor CarColor { get; set; }
        public Driver Driver { get; set; }
    }
}