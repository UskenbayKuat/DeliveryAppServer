﻿using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Enums;

namespace ApplicationCore.Entities.AppEntities
{
    public class Car : BaseEntity
    {
        public int ProductionYear { get; set; }
        public string RegistrationCertificate { get; set; }
        public string CarNumber { get; set; }
        public CarBrand CarBrand { get; set; }
        public CarType CarType { get; set; } 
        public CarColor CarColor { get; set; }
        public bool IsDeleted { get; set; }
    }
}