using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.Clients
{
    public interface ICalculate
    {
        public Task<CreateOrderDto> CalculateAsync(CreateOrderDto model, CancellationToken cancellationToken);
    }
}