using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Values;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands;

namespace PublicApi.Endpoints.Delivery
{
    public class ConfirmHandOver : EndpointBaseAsync.WithRequest<ConfirmHandOverCommand>.WithActionResult
    {
        private readonly IOrderCommand _orderCommand;
        private readonly IMapper _mapper;

        public ConfirmHandOver(IOrderCommand orderCommand, IMapper mapper)
        {
            _orderCommand = orderCommand;
            _mapper = mapper;
        }

        [HttpPost("api/driver/confirmHandOver")]
        public override async Task<ActionResult> HandleAsync([FromBody] ConfirmHandOverCommand request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _orderCommand.ConfirmHandOverAsync(_mapper.Map<ConfirmHandOverDto>(request));
                return new NoContentResult();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }
        }
            
    }
}