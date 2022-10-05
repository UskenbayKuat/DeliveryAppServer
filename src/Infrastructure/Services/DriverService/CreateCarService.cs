using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces.DriverInterfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.DriverService
{
    public class CreateCarService: ICreateCar
    {
        private readonly AppDbContext _db;

        public CreateCarService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ActionResult> CreateAuto(CreateCarInfo info, CancellationToken token)
        {
            if (info == null)
            {
                return new BadRequestResult();
            }
            Car car = new Car()
            {
                DriverId = info.DriverId,
                CarBrandId = info.CarBrandId,
                CarColorId = info.CarColorId,
                CarTypeId = info.CarTypeId,
                LicensePlate = info.LicensePlate,
                ProductionYear = info.ProductionYear,
                RegistrationCertificate = info.RegistrationCertificate
            };
            await _db.Cars.AddAsync(car, token);
            await _db.SaveChangesAsync(token);
            return new ObjectResult(new { CarId = car.Id});
        }
    }
}