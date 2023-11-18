using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Models.Dtos;

namespace ApplicationCore.Interfaces.Drivers
{
    public interface IDriver
    {
        Task AddCarAsync(CreateCarDto dto);
        Task<Driver> GetByUserIdAsync(Guid userId);
    }
}