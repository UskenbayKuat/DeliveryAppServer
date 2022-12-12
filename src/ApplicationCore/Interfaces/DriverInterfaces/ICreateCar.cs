using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface ICreateCar
    {
        public Task<ActionResult> CreateAutoAsync(CreateCarInfo create, string userId, CancellationToken cancellationToken);
    }
}