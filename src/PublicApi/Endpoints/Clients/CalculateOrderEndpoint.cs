using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Models.Dtos;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;

namespace PublicApi.Endpoints.Clients
{
    public class CalculateOrderEndpoint : EndpointBaseAsync.WithRequest<CreateOrderCommand>.WithActionResult
    {
        private readonly ICalculate _calculate;
        private readonly IMapper _mapper;


        public CalculateOrderEndpoint(ICalculate calculate, IMapper mapper)
        {
            _calculate = calculate;
            _mapper = mapper;
        }

        [HttpPost("api/client/calculate")]
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