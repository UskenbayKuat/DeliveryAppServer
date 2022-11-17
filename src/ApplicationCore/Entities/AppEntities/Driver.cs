using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entities.AppEntities.Cars;
using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Entities.AppEntities
{
    public class Driver : BaseEntity
    {
        public string UserId { get;private set; }
        public string IdentificationNumber { get;private set; }
        public string IdentificationSeries { get; private set;}
        public DateTime IdentityCardCreateDate { get; private set;}
        public string DriverLicenceScanPath { get; private set;}
        public string IdentityCardPhotoPath { get; private set;}
        public Car Car { get; private set; }
        public double Rating { get; set; }
        public Driver(string userId, string identificationNumber, string identificationSeries, DateTime identityCardCreateDate, string driverLicenceScanPath, string identityCardPhotoPath)
        {
            UserId = userId;
            IdentificationNumber = identificationNumber;
            IdentificationSeries = identificationSeries;
            IdentityCardCreateDate = identityCardCreateDate;
            DriverLicenceScanPath = driverLicenceScanPath;
            IdentityCardPhotoPath = identityCardPhotoPath;
        }

        public void AddCarr(Car car)
        {
            if (Car is not null)
                throw new BadHttpRequestException("Car is already added");
            Car = car;
        }
    }
}