using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DriverService
{
    public class CreateCarService: ICreateCar
    {
        private readonly AppDbContext _db;

        public CreateCarService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ActionResult> CreateAutoAsync(CreateCarInfo info, string userId, CancellationToken token)
        {
            var driver = _db.Drivers.Include(d => d.Car).First(d => d.UserId == userId);
            var carBrand = await _db.CarBrands.FirstOrDefaultAsync(b => b.Id == info.CarBrandId, token);
            var carColor = await _db.CarColors.FirstOrDefaultAsync(b => b.Id == info.CarColorId, token);
            var carType = await _db.CarTypes.FirstOrDefaultAsync(b => b.Id == info.CarTypeId, token);
            if (driver.Car is not null)
            {
                return new BadRequestObjectResult("Car is already added");
            } 
            var car = new Car
            {
                CarBrand = carBrand,
                CarColor = carColor,
                CarType = carType,
                CarNumber = info.LicensePlate,
                ProductionYear = info.ProductionYear,
                RegistrationCertificate = info.RegistrationCertificate
            };
            driver.Car= car;
            await _db.Cars.AddAsync(car, token); 
            _db.Drivers.Update(driver);
            await _db.SaveChangesAsync(token);
            return new OkObjectResult(car);
        }
    }
}