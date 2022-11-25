using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.Values;
using ApplicationCore.Exceptions;
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

        public async Task<ActionResult> CreateAutoAsync(CreateCarInfo info, string userId, CancellationToken cancellationToken)
        {
            try
            {
                var driver = await _db.Drivers.Include(d => d.Car).FirstAsync(d => d.UserId == userId,cancellationToken);
                var carBrand = await _db.CarBrands.FirstAsync(b => b.Id == info.CarBrandId, cancellationToken);
                var carColor = await _db.CarColors.FirstAsync(b => b.Id == info.CarColorId, cancellationToken);
                var carType = await _db.CarTypes.FirstAsync(b => b.Id == info.CarTypeId, cancellationToken);

                driver.Car = new Car(info.ProductionYear, info.RegistrationCertificate, info.LicensePlate)
                    { CarBrand = carBrand, CarColor = carColor, CarType = carType };
              
                _db.Drivers.Update(driver);
                await _db.SaveChangesAsync(cancellationToken);
                return new OkObjectResult(new{driver.Car});
            }
            catch(CarExistsException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch
            {
                return new BadRequestObjectResult("Not correct data");
            }
        }
    }
}