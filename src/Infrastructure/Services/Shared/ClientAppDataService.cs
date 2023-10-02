using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.Shared
{
    public class ClientAppDataService : IDeliveryAppData<ClientAppDataDto>
    {
        private readonly IAsyncRepository<City> _contextCity;
        private readonly IAsyncRepository<CarType> _contextCarType;
        public ClientAppDataService(IAsyncRepository<City> contextCity, IAsyncRepository<CarType> contextCarType)
        {
            _contextCity = contextCity;
            _contextCarType = contextCarType;
        }
        public async Task<ActionResult> SendDataAsync(CancellationToken cancellationToken)
        {
            var cities = await _contextCity.ListAllAsync(cancellationToken);
            var carTypes = await _contextCarType.ListAllAsync(cancellationToken);
            var info = new ClientAppDataDto
            {
                Cities = cities,
                CarTypes = carTypes
            };
            return new OkObjectResult(info);
        }
    }
}