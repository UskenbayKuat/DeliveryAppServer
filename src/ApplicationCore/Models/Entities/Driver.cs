using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Exceptions;
using ApplicationCore.Models.Entities.Cars;

namespace ApplicationCore.Entities.AppEntities
{
    public class Driver : BaseEntity
    {
        public Driver(string identificationNumber, string identificationSeries, DateTime identityCardCreateDate, string driverLicenceScanPath, string identityCardPhotoPath)
        {
            IdentificationNumber = identificationNumber;
            IdentificationSeries = identificationSeries;
            IdentityCardCreateDate = identityCardCreateDate;
            DriverLicenceScanPath = driverLicenceScanPath;
            IdentityCardPhotoPath = identityCardPhotoPath;
        }
        public string IdentificationNumber { get;private set; }
        public string IdentificationSeries { get; private set;}
        public DateTime IdentityCardCreateDate { get; private set;}
        public string DriverLicenceScanPath { get; private set;}
        public string IdentityCardPhotoPath { get; private set;}
        public User User { get; set; }
        public Guid UserId { get; set; }
        public ICollection<Car> Cars { get; set;}
        public double Rating { get; set; }

        public Driver SetCar(Car car)
        {
            Cars ??= new List<Car>();

            if (Cars.Any(x => !x.IsDeleted))
            {
                throw new CarExistsException();
            }

            Cars.Add(car);
            return this;
        }

    }
}