namespace ApplicationCore.Entities.AppEntities.Cars
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
        public CarBrand CarBrand { get; private set; }
        public CarType CarType { get; private set; }
        public CarColor CarColor { get; private set; }
        public bool IsDeleted { get; private set; }



        public Car AddCarOption(CarBrand carBrand, CarType carType, CarColor carColor)
        {
            CarBrand = carBrand;
            CarType = carType;
            CarColor = carColor;
            return this;
        }

        public void DeleteCar()
        {
            IsDeleted = true;
        }
    }
}