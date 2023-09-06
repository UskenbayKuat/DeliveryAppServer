using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using Infrastructure.Config.Attributes;
using Ardalis.ApiEndpoints;
using ApplicationCore.Interfaces.ClientInterfaces;

namespace PublicApi.Endpoints.Order.Query
{
    [Authorize]
    public class GetHistoryOrder : EndpointBaseAsync.WithoutRequest.WithActionResult
    {
        private readonly IOrderQuery _orderQuery;

        public GetHistoryOrder(IOrderQuery orderQuery)
        {
            _orderQuery = orderQuery;
        }

        [HttpPost("api/driver/historyOrder")]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var orderDtoList =
                    await _orderQuery.GetHistoryAsync(HttpContext.Items["UserId"]?.ToString());
                return Ok(orderDtoList);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Ошибка системы");
            }
        }
    }
}
