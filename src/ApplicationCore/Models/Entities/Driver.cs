using System;
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

        private Car _car;

        public Car Car
        {
            get => _car;
            set
            {
                if (_car is not null)
                {
                    throw new CarExistsException();
                }
                _car = value;
            }
        }

        public double Rating { get; set; }

    }
}