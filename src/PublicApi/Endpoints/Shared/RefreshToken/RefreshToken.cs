using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.RefreshToken
{
    public class RefreshToken : EndpointBaseAsync.WithRequest<RefreshRequest>.WithActionResult
    {
        private readonly IRefreshToken _refreshToken;
        private readonly IMapper _mapper;

        public RefreshToken(IRefreshToken refreshToken, IMapper mapper)
        {
            _refreshToken = refreshToken;
            _mapper = mapper;
        }


        [HttpPost("api/RefreshToken")]
        public override async Task<ActionResult> HandleAsync([FromBody] RefreshRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var accessToken = _refreshToken.RefreshTokenAsync(_mapper.Map<RefreshTokenInfo>(request));
                return new OkObjectResult( new RefreshResponse
                    {
                        AccessToken = await accessToken
                    });
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}