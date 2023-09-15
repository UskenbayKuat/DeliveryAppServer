using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;

namespace ApplicationCore.Interfaces.Clients
{
    public interface IClient
    {
        Task<Client> GetByUserId(string userId);
    }
}