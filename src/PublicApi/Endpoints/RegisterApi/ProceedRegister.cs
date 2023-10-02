using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Register;
using ApplicationCore.Models.Dtos.Register;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Register;

namespace PublicApi.Endpoints.RegisterApi
{
    [Authorize]
    public class ProceedRegister : EndpointBaseAsync.WithRequest<ProceedRegisterCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly IProceedRegistration _proceedRegistration;
        private readonly IValidation _validation;

        public ProceedRegister(IMapper mapper,IValidation validation, IProceedRegistration proceedRegistration)
        {
            _mapper = mapper;
            _validation = validation;
            _proceedRegistration = proceedRegistration;
        }
        
        [HttpPost("api/proceedRegister")]
        public override async Task<ActionResult> HandleAsync([FromBody]ProceedRegisterCommand request, CancellationToken cancellationToken = default)
        {
            return await _proceedRegistration.ProceedRegistration(_mapper.
                Map<ProceedRegistrationDto>(request), HttpContext.Items["UserId"]?.ToString(), cancellationToken);
        }

    }
}