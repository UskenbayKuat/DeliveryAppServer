using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services
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
            var car = _db.Cars.FirstOrDefault(c => c.LicensePlate == info.LicensePlate);
            if (car is not null)
            {
                return new BadRequestResult();
            }

            car = new Car()
            {
                Brand = info.Brand,
                Color = info.Color,
                CarTypeId = info.CarTypeId,
                Model = info.Model,
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