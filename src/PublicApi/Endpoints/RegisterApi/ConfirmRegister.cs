using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Models.Dtos.Register;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Commands.Register;

namespace PublicApi.Endpoints.RegisterApi
{
    public class ConfirmRegister : EndpointBaseAsync.WithRequest<ConfirmRegisterCommand>.WithActionResult
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
        public override async Task<ActionResult> HandleAsync([FromBody]ConfirmRegisterCommand request, CancellationToken cancellationToken = default)
        {
            if (!_validation.ValidationCode(request.SmsCode))
            {
                return BadRequest();
            }
            return await _registration.Confirm(_mapper.
                Map<ConfirmRegistrationDto>(request), cancellationToken);;
        }
    }
}