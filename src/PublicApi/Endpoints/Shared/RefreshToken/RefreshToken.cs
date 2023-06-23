using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Values;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.Shared.RefreshToken
{
    public class RefreshToken : EndpointBaseAsync.WithRequest<RefreshCommand>.WithActionResult
    {
        private readonly IRefreshToken _refreshToken;
        private readonly IMapper _mapper;

        public RefreshToken(IRefreshToken refreshToken, IMapper mapper)
        {
            _refreshToken = refreshToken;
            _mapper = mapper;
        }


        [HttpPost("api/refreshToken")]
        public override async Task<ActionResult> HandleAsync([FromBody] RefreshCommand command,
            CancellationToken cancellationToken = default) => await 
            _refreshToken.RefreshTokenAsync(_mapper.Map<RefreshTokenDto>(command));
    }
}