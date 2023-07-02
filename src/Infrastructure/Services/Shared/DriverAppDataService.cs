using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.SharedInterfaces;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.Shared
{
    public class DriverAppDataService : IDeliveryAppData<DriverAppDataDto>
    {
        private readonly IAsyncRepository<City> _contextCity;
        private readonly IAsyncRepository<Kit> _contextKit;
        private readonly IAsyncRepository<CarBrand> _contextCarBrand;
        private readonly IAsyncRepository<CarType> _contextCarType;
        private readonly IAsyncRepository<CarColor> _contextCarColor;

        public DriverAppDataService(
            IAsyncRepository<City> contextCity, 
            IAsyncRepository<Kit> contextKit, 
            IAsyncRepository<CarBrand> contextCarBrand, 
            IAsyncRepository<CarType> contextCarType, 
            IAsyncRepository<CarColor> contextCarColor)
        {
            _contextCity = contextCity;
            _contextKit = contextKit;
            _contextCarBrand = contextCarBrand;
            _contextCarType = contextCarType;
            _contextCarColor = contextCarColor;
        }
        public async Task<ActionResult> SendDataAsync(CancellationToken cancellationToken)
        {
            var info = new DriverAppDataDto
            {
                Cities =  await _contextCity.ListAllAsync(cancellationToken),
                Kits =  await _contextKit.ListAllAsync(cancellationToken),
                CarBrands =  await _contextCarBrand.ListAllAsync(cancellationToken),
                CarTypes =  await _contextCarType.ListAllAsync(cancellationToken),
                CarColors =  await _contextCarColor.ListAllAsync(cancellationToken)
            };
            return new OkObjectResult(info);
        }
    }
}