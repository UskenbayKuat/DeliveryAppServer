using System.Threading.Tasks;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;

namespace ApplicationCore.Interfaces.Shared
{
    public interface IState
    {
        Task<State> GetByStateAsync(GeneralState state);
    }
}