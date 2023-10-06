using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Dtos.Shared;
using Ardalis.ApiEndpoints;
using Infrastructure.Config;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Endpoints.Shared
{
    [Authorize]
    public class LocationApi : EndpointBaseAsync.WithRequest<LocationDto>.WithActionResult
    {
        private readonly IOrderQuery _orderQuery;

        public LocationApi(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        [HttpPost("api/currentLocation")]
        public async override Task<ActionResult> HandleAsync(LocationDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _orderQuery.GetCurrentLocationAsync(request);
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
