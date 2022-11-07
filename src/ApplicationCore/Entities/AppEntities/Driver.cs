using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities.AppEntities
{
    public class Driver : BaseEntity
    {
        public string UserId { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationSeries { get; set; }
        public DateTime IdentityCardCreateDate { get; set; }
        public string DriverLicenceScanPath { get; set; }
        public string IdentityCardPhotoPath { get; set; }
        public Car Car { get; set; }
        public double Rating { get; set; }
    }
}