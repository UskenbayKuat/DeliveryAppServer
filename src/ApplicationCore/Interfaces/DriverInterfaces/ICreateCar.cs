using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface ICreateCar
    {
        public Task<ActionResult> CreateAuto(CreateCarInfo create, string userId, CancellationToken cancellationToken);
    }
}