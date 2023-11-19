using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos.Shared;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Shared;
using System.Threading;
using System.Threading.Tasks;

namespace PublicApi.Endpoints.Shared
{
    [Authorize]
    public class LoggerApi : EndpointBaseAsync.WithRequest<MobileLoggerCommand>.WithActionResult
    {
        private readonly IMobileLogger _logger;
        private readonly IMapper _mapper;
        public LoggerApi(IMobileLogger logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("api/logger")]
        public async override Task<ActionResult> HandleAsync([FromBody]MobileLoggerCommand request, CancellationToken cancellationToken = default)
        {
            await _logger.AddAsync(_mapper.Map<MobileLoggerDto>(request));
            return NoContent();
        }
    }
}
