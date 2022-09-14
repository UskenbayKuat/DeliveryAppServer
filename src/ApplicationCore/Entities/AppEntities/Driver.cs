using System.Collections.Generic;

namespace ApplicationCore.Entities.AppEntities
{
    public class Driver : BaseEntity
    {
        public string UserId { get; set; }
        public string IdentityCardFaceScanPath { get; set; }
        public string IdentityCardBackScanPath { get; set; }
        public string DrivingLicenceScanPath { get; set; }
        public string DriverPhoto { get; set; }
        public int? CarId { get; set; }
        public Car Car { get; set; }
        public List<Kit> Kits { get; set; }
        public double Rating { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsValid { get; set; }
    }
}