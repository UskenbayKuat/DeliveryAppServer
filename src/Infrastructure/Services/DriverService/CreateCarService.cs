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
            if (driver.CarId is not null || info is null)
            {
                return new BadRequestObjectResult("Car added");
            } 
            var car = new Car
            {
                DriverId = driver.Id,
                CarBrandId = info.CarBrandId,
                CarColorId = info.CarColorId,
                CarTypeId = info.CarTypeId,
                LicensePlate = info.LicensePlate,
                ProductionYear = info.ProductionYear,
                RegistrationCertificate = info.RegistrationCertificate
            };
            _db.Cars.Add(car);       
            driver.CarId = _db.Entry(car).Property(c => c.Id).CurrentValue;
            _db.Drivers.Update(driver);
            await _db.SaveChangesAsync(token);
            return new OkObjectResult(car);
        }
    }
}