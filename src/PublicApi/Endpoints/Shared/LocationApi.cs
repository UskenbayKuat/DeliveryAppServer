using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Dtos.Shared;
using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Endpoints.Shared
{
    [Authorize]
    public class LocationApi : EndpointBaseAsync.WithRequest<LocationCommand>.WithActionResult
    {
        private readonly IOrderQuery _orderQuery;

        public LocationApi(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        [HttpPost("api/currentLocation")]
        public async override Task<ActionResult> HandleAsync([FromBody]LocationCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _orderQuery.GetCurrentLocationAsync(request.OrderId);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Ошибка: {ex.Message}");
            }
            catch
            {
                return BadRequest($"Ошибка система");
            }
        }
    }
}
