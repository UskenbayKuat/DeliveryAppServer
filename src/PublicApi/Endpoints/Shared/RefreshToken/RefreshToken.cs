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
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public RefreshToken(IRefreshTokenService refreshTokenService, IMapper mapper)
        {
            _refreshTokenService = refreshTokenService;
            _mapper = mapper;
        }


        [HttpPost("api/RefreshToken")]
        public override async Task<ActionResult> HandleAsync([FromBody]RefreshRequest request, CancellationToken cancellationToken = default)
        {
            var accessToken = await _refreshTokenService.RefreshToken(_mapper.Map<RefreshTokenInfo>(request));
            if (string.IsNullOrEmpty(accessToken))
            {
                return new BadRequestResult();
            }
            return new OkObjectResult(new RefreshResponse { AccessToken = accessToken });

        }
    }
}