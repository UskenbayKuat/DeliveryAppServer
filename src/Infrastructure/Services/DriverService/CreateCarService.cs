using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
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
            try
            {
                var driver = _db.Drivers.Include(d => d.Car).First(d => d.UserId == userId);
                var carBrand = await _db.CarBrands.FirstAsync(b => b.Id == info.CarBrandId, token);
                var carColor = await _db.CarColors.FirstAsync(b => b.Id == info.CarColorId, token);
                var carType = await _db.CarTypes.FirstAsync(b => b.Id == info.CarTypeId, token);

               driver.AddCarr(new Car(info.ProductionYear, info.RegistrationCertificate, info.LicensePlate).AddCarOption(carBrand, carType, carColor));
                _db.Drivers.Update(driver);
                await _db.SaveChangesAsync(token);
                return new OkObjectResult(new{driver.Car});
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}