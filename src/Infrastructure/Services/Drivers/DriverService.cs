using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Drivers;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Entities.Cars;
using ApplicationCore.Specifications.Deliveries;

namespace Infrastructure.Services.Drivers
{
    public class DriverService : IDriver
    {
        private readonly IAsyncRepository<Driver> _context;
        private readonly IAsyncRepository<CarBrand> _contextCarBrand;
        private readonly IAsyncRepository<CarColor> _contextCarColor;
        private readonly IAsyncRepository<CarType> _contextCarType;

        public DriverService(
            IAsyncRepository<Driver> context,
            IAsyncRepository<CarBrand> contextCarBrand,
            IAsyncRepository<CarColor> contextCarColor,
            IAsyncRepository<CarType> contextCarType)
        {
            _context = context;
            _contextCarBrand = contextCarBrand;
            _contextCarColor = contextCarColor;
            _contextCarType = contextCarType;
        }

        public async Task AddCarAsync(CreateCarDto dto)
        {
            var driverSpec = new DriverWithCarSpecification(dto.UserId);
            var driver = await _context.FirstOrDefaultAsync(driverSpec);
            var carBrand = await _contextCarBrand.FirstOrDefaultAsync(b => b.Id == dto.CarBrandId);
            var carColor = await _contextCarColor.FirstOrDefaultAsync(b => b.Id == dto.CarColorId);
            var carType = await _contextCarType.FirstOrDefaultAsync(b => b.Id == dto.CarTypeId);
            driver.Car = new Car(dto.ProductionYear, dto.RegistrationCertificate, dto.LicensePlate)
            { CarBrand = carBrand, CarColor = carColor, CarType = carType };
            await _context.UpdateAsync(driver);
        }

        public async Task<Driver> GetByUserIdAsync(Guid userId)
        {
            var driverSpec = new DriverWithCarSpecification(userId);
            return await _context.FirstOrDefaultAsync(driverSpec);
        }
    }
}