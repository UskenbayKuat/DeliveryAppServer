using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Models.Dtos.Orders;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;
using PublicApi.Commands.Orders.Models;

namespace PublicApi.Endpoints.Clients.Command
{
    public class CalculateOrder : EndpointBaseAsync.WithRequest<CreateOrderCommand>.WithActionResult
    {
        private readonly ICalculate _calculate;
        private readonly IMapper _mapper;


        public CalculateOrder(ICalculate calculate, IMapper mapper)
        {
            _calculate = calculate;
            _mapper = mapper;
        }

        [HttpPost("api/calculate")]
        public override async Task<ActionResult> HandleAsync([FromBody] CreateOrderCommand request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var model = await _calculate.CalculateAsync(_mapper.Map<CreateOrderDto>(request), cancellationToken);
                return new OkObjectResult(model);
            }
            catch(ArgumentException ex)
            {
                return BadRequest($"Не правильное рассчет калькулаций: {ex.Message}");
            }
        }
    }
}