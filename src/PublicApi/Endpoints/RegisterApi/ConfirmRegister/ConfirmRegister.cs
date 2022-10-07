using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.RegisterInterfaces;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Endpoints.RegisterApi.ConfirmRegister
{
    public class ConfirmRegister : EndpointBaseAsync.WithRequest<ConfirmRegisterCommand>.WithActionResult<ConfirmRegisterResult>
    {
        private readonly IMapper _mapper;
        private readonly IRegistration _registration;
        private readonly IValidation _validation;

        public ConfirmRegister(IMapper mapper,IValidation validation, IRegistration registration)
        {
            _mapper = mapper;
            _validation = validation;
            _registration = registration;
        }
        
        [HttpPost("api/confirmRegister")]
        public override async Task<ActionResult<ConfirmRegisterResult>> HandleAsync([FromBody]ConfirmRegisterCommand request, CancellationToken cancellationToken = default)
        {
            if (!_validation.ValidationCode(request.SmsCode))
            {
                return BadRequest();
            }
            var info = await _registration.Confirm(_mapper.
                Map<ConfirmRegistrationInfo>(request), cancellationToken);

            var result = _mapper.Map<ConfirmRegisterResult>(info);
            return new OkObjectResult(result);
        }
    }
}