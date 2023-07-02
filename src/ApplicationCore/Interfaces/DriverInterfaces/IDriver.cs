using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Values;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IDriver
    {
        Task AddCarAsync(CreateCarDto dto);
        Task<Driver> GetByUserIdAsync(string userId);
    }
}