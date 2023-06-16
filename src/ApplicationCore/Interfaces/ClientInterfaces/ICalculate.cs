using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface ICalculate
    {
        public Task<CreateOrderDto> CalculateAsync(CreateOrderDto model,CancellationToken cancellationToken);
    }
}