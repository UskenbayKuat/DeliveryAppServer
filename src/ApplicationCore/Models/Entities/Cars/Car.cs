using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities.Cars;

namespace ApplicationCore.Models.Entities.Cars
{
    public class Car : BaseEntity
    {
        public Car(int productionYear, string registrationCertificate, string carNumber)
        {
            ProductionYear = productionYear;
            RegistrationCertificate = registrationCertificate;
            CarNumber = carNumber;
        }
        public int ProductionYear { get; private set; }
        public string RegistrationCertificate { get; private set; }
        public string CarNumber { get; private set; }
        public CarBrand CarBrand { get; set; }
        public CarType CarType { get; set; }
        public CarColor CarColor { get; set; }

        public void DeleteCar()
        {
            IsDeleted = true;
        }
    }
}