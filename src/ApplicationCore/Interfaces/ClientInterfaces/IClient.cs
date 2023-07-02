using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IClient
    {
        Task<Client> GetByUserId(string userId);
    }
}