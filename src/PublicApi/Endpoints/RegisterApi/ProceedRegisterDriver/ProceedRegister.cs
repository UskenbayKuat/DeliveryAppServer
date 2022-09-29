using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.RegisterInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.RegisterApi.ProceedRegisterDriver
{
    public class ProceedRegister : EndpointBaseAsync.WithRequest<ProceedRegisterCommand>.WithActionResult<ProceedRegisterResult>
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
        public override async Task<ActionResult<ProceedRegisterResult>> HandleAsync([FromBody]ProceedRegisterCommand request, CancellationToken cancellationToken = default)
        {
            return await _proceedRegistration.ProceedRegistration(_mapper.
                Map<ProceedRegistrationInfo>(request), cancellationToken);
        }

    }
}