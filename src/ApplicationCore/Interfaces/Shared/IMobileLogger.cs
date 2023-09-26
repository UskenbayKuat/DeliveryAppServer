using ApplicationCore.Models.Dtos.Shared;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Shared
{
    public interface IMobileLogger
    {
        Task AddAsync(MobileLoggerDto dto);
    }
}
