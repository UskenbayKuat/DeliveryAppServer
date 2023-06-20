using System.Threading.Tasks;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Values.Enums;

namespace ApplicationCore.Interfaces.StateInterfaces
{
    public interface IState
    {
        Task<State> GetByStateAsync(GeneralState state);
    }
}