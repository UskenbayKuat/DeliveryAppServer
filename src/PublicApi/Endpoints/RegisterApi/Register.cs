﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Register;
using ApplicationCore.Models.Values;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLog;
using PublicApi.Commands.Register;

namespace PublicApi.Endpoints.RegisterApi

{
    public class Register : EndpointBaseAsync.WithRequest<RegisterCommand>.WithActionResult
    {
        private readonly IMapper _mapper;
        private readonly IRegistration _registration;
        private readonly IValidation _validation;
        private readonly Logger _logger = LogManager.GetLogger("AppLogsRule");
        
        public Register(IMapper mapper, IRegistration registration, IValidation validation)
        {
            _mapper = mapper;
            _registration = registration;
            _validation = validation;
        }
        
        [HttpPost("api/register")]
        public override async Task<ActionResult> HandleAsync([FromBody]RegisterCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!_validation.ValidationMobileNumber(request.PhoneNumber))
                {
                    _logger.Info("POST Обращение в RegisterApi номер телефона не прошел валидацию");
                    return BadRequest();
                }
                _logger.Info("POST Обращение в RegisterApi");
                await _registration.RegisterAsync(_mapper
                    .Map<RegistrationDto>(request));
                return NoContent();
            }
            catch(ArgumentException ex)
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